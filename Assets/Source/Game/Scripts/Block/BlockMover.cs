using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static YG.LangYGAdditionalText;

public class BlockMover : MonoBehaviour
{
    [SerializeField] private float _time = 1f;
    [SerializeField] private float _distance = 2f;
    [SerializeField] private AnimationCurve _curve = new AnimationCurve(new[] { new Keyframe(0, 0, 2, 2), new Keyframe(1, 1, 0, 0) });

    private Transform _transform;
    private Coroutine _coroutine;

    private Cell _cell;

    public bool IsMoving { get; private set; } = false;

    private void Start()
    {
        _transform = transform;

        if (TryGetComponent(out Block block))
        {
            _cell = block.Cell;
        }
    }

    public void SetupMove(DirectionType side)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        Cell targetCell = GetTargetCell(side);

        if (targetCell != null && !targetCell.IsOccupied())
        {
            _coroutine = StartCoroutine(MoveToTarget(targetCell));
        }
        else if (targetCell == null)
        {
            _coroutine = StartCoroutine(MoveInDirection(side));
        }
        else if (targetCell.IsOccupied())
        {
            BlockShaker shaker = GetComponent<BlockShaker>();
            shaker.Shake();
        }
    }

    private IEnumerator MoveToTarget(Cell target)
    {
        IsMoving = true;
        Vector3 startPosition = _transform.position;
        Vector3 endPosition = target.transform.position;

        float timer = 0;

        while (timer < _time)
        {
            float way = timer / _time;
            float wayCurve = _curve.Evaluate(way);

            _transform.position = Vector3.Lerp(startPosition, endPosition, wayCurve);
            timer += Time.deltaTime;

            yield return null;
        }

        _transform.position = endPosition;

        TryMove(target);

        IsMoving = false;
    }

    private IEnumerator MoveInDirection(DirectionType direction)
    {
        IsMoving = true;
        Vector3 startPosition = _transform.position;
        Vector3Int moveDirection = direction.ToVector3Int();
        Vector3 targetPosition = startPosition + (Vector3)moveDirection * _distance;

        float timer = 0;

        while (timer < _time)
        {
            float way = timer / _time;
            float wayCurve = _curve.Evaluate(way);

            _transform.position = Vector3.Lerp(startPosition, targetPosition, wayCurve);
            timer += Time.deltaTime;

            yield return null;
        }

        _transform.position = targetPosition;
        _cell.SetFree();

        IsMoving = false;
        Destroy(gameObject);
    }

    private void TryMove(Cell targetCell)
    {
        if (targetCell != null && !targetCell.IsOccupied())
        {
            _cell.SetFree();
            targetCell.SetOccupy(GetComponent<Block>());
            _cell = targetCell;
        }
    }

    private Cell GetTargetCell(DirectionType direction)
    {
        Vector3Int moveDirection = direction.ToVector3Int();
        Vector3Int newPosition = new Vector3Int(_cell.Position.x + moveDirection.x, _cell.Position.y + moveDirection.y, _cell.Position.z + moveDirection.z);

        return _cell.GetGrid().GetCell(newPosition);
    }
}
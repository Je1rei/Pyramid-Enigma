using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static YG.LangYGAdditionalText;

public class BlockMover : MonoBehaviour
{
    [SerializeField] private float _time = 1f;
    [SerializeField] private float _distance = 1f;
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

    public void SetupMove() // что-то с обработкой перемещения в клетку, проверка на свободную клетку, непонятно что
    {
        if (IsMoving) return;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        if (CanMoveForward())
        {
            _coroutine = StartCoroutine(MoveForward());
        }
        else
        {
            BlockShaker shaker = GetComponent<BlockShaker>();
            shaker.Shake();
        }
    }

    private IEnumerator MoveForward()
    {
        IsMoving = true;
        Vector3 startPosition = _transform.position;
        Vector3 targetPosition = _transform.position + _transform.forward * _distance;

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

        TryMove(GetTargetCell());

        IsMoving = false;
    }

    private bool CanMoveForward()
    {
        RaycastHit hit;

        if (Physics.Raycast(_transform.position, _transform.forward, out _, _distance))
        {
            return false;
        }

        return true;
    }

    private void TryMove(Cell targetCell)
    {
        if (targetCell != null)
        {
            if (!targetCell.IsOccupied())
            {
                _cell.SetFree();
                targetCell.SetOccupy(GetComponent<Block>());
                _cell = targetCell;
            }
        }
        else if(targetCell == null)
        {
            _cell.SetFree();
            Destroy(gameObject);
        }
    }

    private Cell GetTargetCell()
    {
        Vector3Int offset = Vector3Int.RoundToInt(_transform.forward);
        Vector3Int newPosition = new Vector3Int(_cell.Position.x + offset.x, _cell.Position.y + offset.y, _cell.Position.z + offset.z);

        return _cell.GetGrid().GetCell(newPosition);
    }
}
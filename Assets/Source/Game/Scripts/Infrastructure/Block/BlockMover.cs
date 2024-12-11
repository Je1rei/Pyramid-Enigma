using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class BlockMover : MonoBehaviour
{
    [SerializeField] private float _time = 0.01f;
    [SerializeField] private float _distance = 1f;
    [SerializeField] private float _destroyDelay = 1f;
    [SerializeField] private float _durationRotate = 0.5f;
    [SerializeField] private int _maxMoveDistance = 30;

    private Cell _cell;
    private Block _block;
    private InputPause _inputPauser;
    private BlockShaker _shaker;
    private BlockExploder _exploder;

    private Tween _tween;
    private Sequence _sequence;

    public bool IsMoving { get; private set; } = false;

    public event Action Moved;

    private void Start()
    {
        if (TryGetComponent(out _block))
        {
            _cell = _block.Cell;
        }

        _inputPauser = ServiceLocator.Current.Get<InputPause>();
        _shaker = GetComponent<BlockShaker>();
        _exploder = GetComponent<BlockExploder>();
    }

    public void SetupMove()
    {
        if (IsMoving)
            return;

        if (CanMove())
            Move();
        else
            _shaker.Shake();
    }

    private void Move()
    {
        IsMoving = true;
        _block.PlaySound();

        Vector3 targetPosition = GetFurthestPosition();

        _tween = transform.DOMove(targetPosition, _time).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            Cell targetCell = GetTargetCell(targetPosition);

            if (targetCell == null)
            {
                DestroyAfterDelay();
            }
            else
            {
                TryMove(targetCell);
                TryRotate();
            }

            IsMoving = false;
            Moved?.Invoke();
        });
    }

    private void TryRotate()
    {
        if (_sequence == null)
            _sequence = DOTween.Sequence();

        Vector3Int blockDirection = _block.ForwardDirection;
        Vector3Int neighborPosition = _cell.Position + blockDirection;
        Cell neighborCell = _cell.GetGrid().GetCell(neighborPosition);

        if (neighborCell != null && neighborCell.IsOccupied())
        {
            if (neighborCell.Occupied.TryGetComponent(out Block neighborBlock))
            {
                DirectionType oppositeDirection = _block.GetAllowedDirection().ToOpposite();

                if (neighborBlock.GetAllowedDirection() == oppositeDirection)
                {
                    _block.SetAllowedDirection(oppositeDirection);
                    _block.UpdateForwardDirection();

                    _sequence.Append(_block.transform.DORotateQuaternion(Quaternion.LookRotation(neighborBlock.ForwardDirection), _durationRotate)
                        .SetEase(Ease.InOutQuad));
                }
            }
        }

        _sequence.OnComplete(() => _sequence.Kill());
    }

    private Vector3 GetFurthestPosition()
    {
        float offset = 0.5f;
        float maxDistance = _distance * _maxMoveDistance;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, maxDistance);

        Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject == gameObject)
                continue;

            Vector3 hitPosition = RoundToGrid(hit.point - transform.forward.normalized * offset);
            Cell cell = _cell.GetGrid().GetCell(Vector3Int.FloorToInt(hitPosition));

            if (cell == null || cell.IsOccupied())
                return hitPosition - transform.forward.normalized * _distance;

            return hitPosition;
        }

        return RoundToGrid(transform.position + transform.forward * maxDistance);
    }

    private Vector3 RoundToGrid(Vector3 position)
    {
        return new Vector3(
            Mathf.Round(position.x),
            Mathf.Round(position.y),
            Mathf.Round(position.z)
        );
    }

    private bool CanMove()
    {
        bool canMove = Physics.Raycast(transform.position, transform.forward, _distance) == false;
        Debug.Log($"CanMove: {canMove}");

        return canMove;
    }

    private void DestroyAfterDelay()
    {
        DOTween.To(() => 0, x => { }, 1, _destroyDelay).OnComplete(() => { DestroyBlock(); });
    }

    private void TryMove(Cell targetCell)
    {
        if (targetCell == null)
        {
            return;
        }

        if (targetCell.IsOccupied())
        {
            return;
        }

        _cell.SetFree();
        _cell = targetCell;
        _cell.SetOccupy(_block);
    }

    private void DestroyBlock()
    {
        Destroy(gameObject);
    }

    private Cell GetTargetCell(Vector3 targetPosition)
    {
        Vector3Int gridPosition = Vector3Int.FloorToInt(targetPosition);
        Cell targetCell = _cell.GetGrid().GetCell(gridPosition);

        return targetCell != null && targetCell.IsOccupied() == false ? targetCell : null;
    }

    public void Explode()
    {
        if (_exploder.TryExplode() && IsMoving == false)
        {
            _cell.SetFree();
            _block.SetCurrentCell(null);
            Moved?.Invoke();
        }
    }
}
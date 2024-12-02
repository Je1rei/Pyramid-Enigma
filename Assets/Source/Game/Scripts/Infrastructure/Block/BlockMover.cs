using DG.Tweening;
using System;
using UnityEngine;

public class BlockMover : MonoBehaviour
{
    [SerializeField] private float _time = 0.5f;
    [SerializeField] private float _distance = 1f;
    [SerializeField] private float _destroyDelay = 1f;
    [SerializeField] private float _durationRotate = 0.5f;
    [SerializeField] private int _maxMoveDistance = 30;

    private Cell _cell;
    private Block _block;

    public bool IsMoving { get; private set; } = false;

    public event Action Moved;

    private void Start()
    {
        if (TryGetComponent(out _block))
        {
            _cell = _block.Cell;
        }
    }

    public void SetupMove()
    {
        if (IsMoving) return;

        if (CanMove())
        {
            Move();
        }
        else
        {
            BlockShaker shaker = GetComponent<BlockShaker>();
            shaker.Shake();
        }
    }

    private void Move()
    {
        IsMoving = true;
        _block.PlaySound();
        _cell.SetFree();

        Vector3 targetPosition = GetFurthestPosition();

        transform.DOMove(targetPosition, _time).SetEase(Ease.OutQuad).OnComplete(() =>
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
                IsMoving = false;
            }

            Moved?.Invoke();
        });
    }

    private void TryRotate()
    {
        Vector3Int blockDirection = _block.ForwardDirection;
        Vector3Int neighborPosition = _cell.Position + blockDirection;

        Cell neighborCell = _cell.GetGrid().GetCell(neighborPosition);

        if (neighborCell != null && neighborCell.IsOccupied())
        {
            if (neighborCell.Occupied.TryGetComponent(out Block neighborBlock))
            {
                if (neighborBlock.ForwardDirection == -_block.ForwardDirection)
                {
                    _block.SetAllowedDirection(neighborBlock.RandomizeDirection());
                    _block.UpdateForwardDirection();

                    DOTween.Sequence()
                        .Append(_block.transform.DORotateQuaternion(Quaternion.LookRotation(neighborBlock.ForwardDirection), _durationRotate))
                        .SetEase(Ease.InOutQuad);
                }
            }
        }
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
        return Physics.Raycast(transform.position, transform.forward, _distance) == false;
    }

    private void TryMove(Cell targetCell)
    {
        if (targetCell != null && !targetCell.IsOccupied())
        {
            _cell = targetCell;
            _cell.SetOccupy(_block);
        }
    }

    private void DestroyAfterDelay()
    {
        Invoke(nameof(DestroyBlock), _destroyDelay);
    }

    private void DestroyBlock()
    {
        Destroy(gameObject);
    }

    private Cell GetTargetCell(Vector3 targetPosition)
    {
        Cell targetCell = _cell.GetGrid().GetCell(Vector3Int.FloorToInt(targetPosition));

        return targetCell != null && targetCell.IsOccupied() == false ? targetCell : null;
    }
}
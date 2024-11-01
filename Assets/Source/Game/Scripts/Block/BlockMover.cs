using DG.Tweening;
using UnityEngine;

public class BlockMover : MonoBehaviour
{
    [SerializeField] private float _time = 0.5f;
    [SerializeField] private float _distance = 1f;

    private Cell _cell;
    private Block _block;

    public bool IsMoving { get; private set; } = false;

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

        if (CanMoveForward())
        {
            MoveForward();
        }
        else
        {
            BlockShaker shaker = GetComponent<BlockShaker>();
            shaker.Shake();
        }
    }

    private void MoveForward()
    {
        IsMoving = true;
        Vector3 targetPosition = transform.position + transform.forward * _distance;

        transform.DOMove(targetPosition, _time).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            TryMove(GetTargetCell());
            IsMoving = false;
        });
    }

    private bool CanMoveForward()
    {
        if (Physics.Raycast(transform.position, transform.forward, out _, _distance))
        {
            return false;
        }

        return true;
    }

    private void TryMove(Cell targetCell)
    {
        if (targetCell != null)
        {
            if (targetCell.IsOccupied() == false)
            {
                _cell.SetFree();
                _cell = targetCell;
                _cell.SetOccupy(_block);
            }
        }
        else
        {
            _cell.SetFree();
            Destroy(gameObject);
        }
    }

    private Cell GetTargetCell()
    {
        Vector3Int offset = _block.ForwardDirection;
        Vector3Int newPosition = _cell.Position + offset;

        return _cell.GetGrid().GetCell(newPosition);
    }
}
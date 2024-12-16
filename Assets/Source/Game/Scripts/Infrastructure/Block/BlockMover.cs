using DG.Tweening;
using System;
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

    private int _countEmptyCells;

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
        _countEmptyCells = 0;
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

        Vector3Int gridDirection = _block.ForwardDirection;
        Vector3 targetPosition = transform.position + (Vector3)gridDirection * _countEmptyCells; // ??????????

        _tween = transform.DOMove(targetPosition, _time).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            Cell targetCell = GetTargetCell();

            if (targetCell == null)
            {
                DestroyAfterDelay();
            }
            else
            {
                TryMove(targetCell);
                //TryRotate();
            }

            IsMoving = false;
            Moved?.Invoke();
        });
    }

    private void TryRotate()
    {
        if (_sequence == null)
            _sequence = DOTween.Sequence();

        Vector3Int direction = Vector3Int.RoundToInt(transform.forward);
        Vector3Int neighborPosition = _cell.Position + direction;
        Cell neighborCell = _cell.GetGrid().GetCell(neighborPosition);

        if (neighborCell != null && neighborCell.IsOccupied())
        {
            if (neighborCell.IsOccupied())
            {
                DirectionType oppositeDirection = _block.GetAllowedDirection().ToOpposite();
                Block neighborBlock = neighborCell.Occupied;

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

    private int CountEmptyCells()
    {
        int count = 0;

        Vector3Int gridDirection = _block.ForwardDirection;
        Vector3Int position = _cell.Position;

        Debug.Log($"(CountEmptyCells) gridDirection -> {gridDirection}, cellPos -> {position}");

        for (int i = 1; i <= _maxMoveDistance; i++)
        {
            Vector3Int nextPosition = position + gridDirection * i;
            Cell nextCell = _cell.GetGrid().GetCell(nextPosition);

            if (nextCell != null)
            {
                if (nextCell.IsOccupied())
                    break;

                count++;
            }
            else
            {
                count = _maxMoveDistance;
            }
        }

        return count;
    }

    private bool CanMove()
    {
        bool canMove = false;
        _countEmptyCells = CountEmptyCells();

        if (_countEmptyCells <= 0)
            canMove = false;
        else
            canMove = true;

        return canMove;
    }

    private void DestroyAfterDelay()
    {
        _block.SetCurrentCell(null);
        _cell.SetFree();
        DOTween.To(() => 0, x => { }, 1, _destroyDelay).OnComplete(() => { DestroyBlock(); });
    }

    private void TryMove(Cell targetCell)
    {
        _cell.SetFree();
        _cell = targetCell;
        _cell.SetOccupy(_block);
    }

    private void DestroyBlock()
    {
        Destroy(gameObject);
    }

    private Cell GetTargetCell() //не проверяется нормально
    {
        Vector3Int gridDirection = _block.ForwardDirection;
        Vector3Int gridPosition = _cell.Position + gridDirection * _countEmptyCells;

        Cell targetCell = _cell.GetGrid().GetCell(gridPosition);

        Debug.Log($"(GetTargetCell) gridDirection -> {gridDirection}, gridPosition -> {gridPosition}, targetCell -> {targetCell?.Position}");

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
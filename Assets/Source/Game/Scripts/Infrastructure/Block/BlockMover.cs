using System;
using DG.Tweening;
using UnityEngine;

namespace Source.Game.Scripts
{
    public class BlockMover : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _destroyDelay = 0.1f;
        [SerializeField] private int _maxMoveDistance = 30;

        private int _countEmptyCells;
        private Color _moveColor;
        private Cell _cell;
        private Block _block;
        private BlockShaker _shaker;
        private BlockExploder _exploder;
    
        public event Action<BlockMover> Released;
    
        public bool IsMoving { get; private set; } = false;

        public void Init()
        {
            if (TryGetComponent(out _block))
            {
                _cell = _block.Cell;
            }
        
            _shaker = GetComponent<BlockShaker>();
            _exploder = GetComponent<BlockExploder>();
            _countEmptyCells = 0;

            _moveColor = Color.green;
            _shaker.Init();
            _exploder.Init(_block.Renderer);
        }

        public void SetupMove()
        {
            if (IsMoving)
            {
                return;
            }

            if (CanMove())
            {
                Move();
            }
            else
            {
                _shaker.Shake();
            }
        }
    
        public void Explode()
        {
            if (_exploder.TryExplode() && IsMoving == false)
            {
                _cell.SetFree();
                _block.SetCurrentCell(null);
                Released?.Invoke(this);
            }
        }
    
        private void Move()
        {
            IsMoving = true;
            _block.PlaySound();
            _block.Renderer.material.color = _moveColor;

            Grid grid = _cell.GetGrid();
            Vector3Int blockForwardDirection = _block.ForwardDirection;
            Vector3 targetPosition = grid.transform.rotation * (blockForwardDirection * _countEmptyCells);

            float duration = ClampDuration(targetPosition.magnitude);
            _cell.SetFree();

            if (TryGetTargetCell(out Cell targetCell))
            {
                OccupieTarget(targetCell);
            }

            transform.DOMove(transform.position + targetPosition, duration).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                if (targetCell == null)
                {
                    DestroyAfterDelay();
                }

                _block.ResetColor();
                IsMoving = false;
                Released?.Invoke(this);
            });
        }

        private float ClampDuration(float magnitude)
        {
            const float sqrtConst = 0.5f;
            const float minDuration = 0.1f;
            const float maxDuration = 1f;

            float distance = magnitude;
            float baseDuration = distance / _speed;
            float duration = Mathf.Clamp(Mathf.Pow(baseDuration, sqrtConst), minDuration, maxDuration);

            return duration;
        }

        private int CountEmptyCells()
        {
            int count = 0;

            Vector3Int gridDirection = _block.GetAllowedDirection().ToVector3Int();
            Vector3Int position = _cell.Position;

            for (int i = 1; i <= _maxMoveDistance; i++)
            {
                Vector3Int nextPosition = position + gridDirection * i;
                Cell nextCell = _cell.GetGrid().GetCell(nextPosition);

                if (nextCell != null)
                {
                    if (nextCell.IsOccupied())
                    {
                        break;
                    }

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
            _countEmptyCells = CountEmptyCells();

            return _countEmptyCells > 0;
        }

        private void DestroyAfterDelay()
        {
            _block.SetCurrentCell(null);
            Destroy(gameObject, _destroyDelay);
        }

        private void OccupieTarget(Cell targetCell)
        {
            _cell = targetCell;
            _cell.SetOccupy(_block);
        }

        private bool TryGetTargetCell(out Cell cell)
        {
            Vector3Int gridDirection = _block.ForwardDirection;
            Vector3Int gridPosition = _cell.Position + gridDirection * _countEmptyCells;

            cell = _cell.GetGrid().GetCell(gridPosition);

            return cell != null;
        }
    }
}
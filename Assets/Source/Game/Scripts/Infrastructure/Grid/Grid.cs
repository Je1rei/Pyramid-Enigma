using System;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(GridFactory), typeof(AudioSource))]
public class Grid : MonoBehaviour
{
    private GridData _data;
    private GridFactory _factory;
    private GridRotator _rotator;

    private Cell[,,] _grid;
    private Vector3 _center;
    private Sequence _sequence;

    public event Action BlocksReleased;
    
    public GridRotator Rotator => _rotator;
    public GridData Data => _data;
    public Vector3 Center => _center;

    public void Init(GridData data)
    {
        _data = data;
        _rotator = GetComponent<GridRotator>();
        _factory = GetComponent<GridFactory>();
        _center = Vector3.zero.CalculateCenter(_data.Width, _data.Height, _data.Length, _data.CellSize);

        _factory.Init();
        Create();
    }
    
    public Cell GetCell(Vector3Int position)
    {
        if (position.x >= 0 && position.x < _data.Width &&
            position.y >= 0 && position.y < _data.Height &&
            position.z >= 0 && position.z < _data.Length)
        {
            return _grid[position.x, position.y, position.z];
        }

        return null;
    }

    private void BlocksIsReleased(BlockMover _)
    {
        foreach (Cell cell in _grid)
        {
            if (cell.IsOccupied())
            {
                return;
            }
        }

        BlocksReleased?.Invoke();
    }

    private void Create()
    {
        _sequence = DOTween.Sequence();

        _sequence.AppendCallback(() => _grid = _factory.Create(_data, this, _rotator, _center));
        _sequence.AppendCallback(() => Subscribe());
    }

    private void Subscribe()
    {
        foreach (var cell in _grid)
        {
            if (cell.IsOccupied() && cell.Occupied.TryGetComponent(out BlockMover mover))
            {
                mover.Released += BlocksIsReleased;
            }
        }
    }
}
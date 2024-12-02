using System;
using UnityEngine;

[RequireComponent(typeof(GridFactory), typeof(AudioSource))]
public class Grid : MonoBehaviour
{
    private GridData _data;
    private GridFactory _factory;
    private GridRotator _rotator;

    private Cell[,,] _grid;
    private Vector3 _center;

    public GridRotator Rotator => _rotator;
    public GridData Data => _data;
    public Vector3 Center => _center;

    public event Action AllBlocksMoved;

    public void Init(GridData data)
    {
        _data = data;
        _rotator = GetComponent<GridRotator>();
        _factory = GetComponent<GridFactory>();
        _center = Vector3.zero.CalculateCenter(_data.Width, _data.Height, _data.Length, _data.CellSize);

        _factory.Init();
        Create();

        _rotator.Init(_center);
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

    private void BlocksIsEmpty()
    {
        foreach (Cell cell in _grid)
        {
            if (cell.IsOccupied())
            {
                return;
            }
        }

        AllBlocksMoved?.Invoke();
    }

    private void Create()
    {
        _grid = _factory.Create(_data, this, _center);
        TryRotateNeighborBlock();
    }

    private void TryRotateNeighborBlock()
    {
        for (int x = 0; x < _data.Width; x++)
        {
            for (int y = 0; y < _data.Height; y++)
            {
                for (int z = 0; z < _data.Length; z++)
                {
                    Cell cell = _grid[x, y, z];

                    if (cell.IsOccupied() && cell.Occupied)
                    {
                        RotateBlock(cell.Occupied, x, y, z);

                        if (cell.Occupied.TryGetComponent(out BlockMover mover))
                        {
                            mover.Moved += BlocksIsEmpty;
                        }
                    }
                }
            }
        }
    }

    private void RotateBlock(Block block, int x, int y, int z)
    {
        Vector3Int blockDirection = block.ForwardDirection;
        Vector3Int neighborPosition = new Vector3Int(x, y, z) + blockDirection;

        Cell neighborCell = GetCell(neighborPosition);

        if (neighborCell != null && neighborCell.IsOccupied())
        {
            if (neighborCell.Occupied.TryGetComponent(out Block neighborBlock))
            {
                if (neighborBlock.ForwardDirection == -blockDirection)
                {
                    DirectionType newDirection;

                    do
                    {
                        newDirection = neighborBlock.RandomizeDirection();
                    }
                    while (newDirection.ToVector3Int() == neighborBlock.ForwardDirection);

                    neighborBlock.SetAllowedDirection(newDirection);
                    neighborBlock.UpdateForwardDirection();
                }
            }
        }
    }
}
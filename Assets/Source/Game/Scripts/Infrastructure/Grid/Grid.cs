using System;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

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
        InputPause inputPauser = ServiceLocator.Current.Get<InputPause>();
        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() => inputPauser.DeactivateInput());
        sequence.AppendCallback(() => _grid = _factory.Create(_data, this, _center));
        sequence.AppendCallback(() => TryRotateNeighborBlock());
        sequence.OnComplete(() => inputPauser.ActivateInput());
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

                    if (cell.IsOccupied())
                    {
                        if (cell.Occupied)
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

        DetectAndResolveCycles();
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

    private void DetectAndResolveCycles()
    {
        Dictionary<Cell, Cell> directedGraph = new Dictionary<Cell, Cell>();

        foreach (Cell cell in _grid)
        {
            if (cell.IsOccupied())
            {
                Vector3Int forwardDirection = cell.Occupied.ForwardDirection;
                Cell neighbor = GetCell(cell.Position + forwardDirection);

                if (neighbor != null && neighbor.IsOccupied())
                {
                    directedGraph[cell] = neighbor;
                }
            }
        }

        HashSet<Cell> visited = new HashSet<Cell>();
        HashSet<Cell> stack = new HashSet<Cell>();

        foreach (var node in directedGraph.Keys)
        {
            if (IsCyclic(node, directedGraph, visited, stack))
            {
                ResolveCycle(node);
                break;
            }
        }
    }

    private bool IsCyclic(Cell current, Dictionary<Cell, Cell> graph, HashSet<Cell> visited, HashSet<Cell> stack)
    {
        if (stack.Contains(current))
            return true;

        if (visited.Contains(current))
            return false;

        visited.Add(current);
        stack.Add(current);

        if (graph.TryGetValue(current, out Cell neighbor))
            if (IsCyclic(neighbor, graph, visited, stack))
                return true;

        stack.Remove(current);

        return false;
    }

    private void ResolveCycle(Cell startCell)
    {
        if (startCell.Occupied != null)
        {
            DirectionType newDirection;

            do
            {
                newDirection = startCell.Occupied.RandomizeDirection();
            } while (newDirection.ToVector3Int() == startCell.Occupied.ForwardDirection);

            startCell.Occupied.SetAllowedDirection(newDirection);
            startCell.Occupied.UpdateForwardDirection();
        }
    }
}
﻿using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CellFactory))]
public class Grid : MonoBehaviour
{
    [SerializeField] private GridData _data;

    private Cell[,,] _grid;
    private CellFactory _cellFactory;
    private Vector3 _center;

    public Vector3 Center => _center;

    public event Action<BlockMover> BlockCreated;

    private void Awake()
    {
        _cellFactory = GetComponent<CellFactory>();
        Create();
        _center = Vector3.zero.CalculateCenter(_data.Width, _data.Height, _data.Length, _data.CellSize);
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

    private void Create()
    {
        _grid = new Cell[_data.Width, _data.Length, _data.Height];

        for (int x = 0; x < _data.Width; x++)
        {
            for (int z = 0; z < _data.Length; z++)
            {
                for (int y = 0; y < _data.Height; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, z);
                    Cell cell = _cellFactory.Create(_data.CellPrefab, position, this);

                    cell.transform.position = new Vector3(x * _data.CellSize, y * _data.CellSize, z * _data.CellSize);
                    _grid[x, y, z] = cell;

                    Block block = Instantiate(_data.BlockPrefab, cell.transform);
                    block.SetCurrentCell(cell);
                    cell.SetOccupy(block);
                    block.Init();

                    if (block.TryGetComponent(out BlockMover mover))
                    {
                        BlockCreated?.Invoke(mover);
                    }
                }
            }
        }

    }

    private Vector3 CalculateCenter()
    {
        float centerX = (_data.Width - 1) * 0.5f * _data.CellSize;
        float centerY = (_data.Height - 1) * 0.5f * _data.CellSize;
        float centerZ = (_data.Length - 1) * 0.5f * _data.CellSize;

        return new Vector3(centerX, centerY, centerZ);
    }
}
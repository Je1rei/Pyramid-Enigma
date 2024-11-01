using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(CellFactory))]
public class Grid : MonoBehaviour
{
    [SerializeField] private GridData _data;
    [SerializeField] private float _spawnDuration = 1f;
    [SerializeField] private float _delay = 0.1f;

    private Cell[,,] _grid;
    private CellFactory _cellFactory;
    private Vector3 _center;
    private GridRotator _rotator;

    public GridRotator Rotator => _rotator;
    public GridData Data => _data;
    public Vector3 Center => _center;

    public event Action<BlockMover> BlockCreated;

    public void Init()
    {
        _rotator = GetComponent<GridRotator>();
        _cellFactory = GetComponent<CellFactory>();

        Create();
        _center = Vector3.zero.CalculateCenter(_data.Width, _data.Height, _data.Length, _data.CellSize);

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

    private void Create() // вынести в фабрику
    {
        _grid = new Cell[_data.Width, _data.Length, _data.Height];
        Sequence sequence = DOTween.Sequence();

        for (int x = 0; x < _data.Width; x++)
        {
            for (int z = 0; z < _data.Length; z++)
            {
                for (int y = 0; y < _data.Height; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, z);
                    Cell cell = _cellFactory.Create(_data.CellPrefab, position, this);

                    Vector3 randomStartPosition = _center + UnityEngine.Random.onUnitSphere * _data.CellSize * 10;
                    cell.transform.position = randomStartPosition;

                    Block block = Instantiate(_data.BlockPrefab, cell.transform);
                    block.SetCurrentCell(cell);
                    block.Init();
                    cell.SetOccupy(block);

                    if (block.TryGetComponent(out BlockMover mover))
                    {
                        sequence.AppendCallback(() => BlockCreated?.Invoke(mover));
                    }

                    _grid[x, y, z] = cell;

                    Vector3 targetPosition = new Vector3(x * _data.CellSize, y * _data.CellSize, z * _data.CellSize);
                    float delay = _delay * (x + y + z);

                    sequence.Insert(delay, cell.transform.DOMove(targetPosition, _spawnDuration).SetEase(Ease.OutBack));
                    sequence.Insert(delay, cell.transform.DORotate(Vector3.one * 360, _spawnDuration, RotateMode.FastBeyond360));
                }
            }
        }

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

                    if (cell.IsOccupied() && cell.Occupied.TryGetComponent(out Block block))
                    {
                        RotateBlock(block, x, y, z);
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
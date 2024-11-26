using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(CellFactory))]
public class GridFactory : MonoBehaviour
{
    [SerializeField] private float _spawnDuration = 1f;
    [SerializeField] private float _delay = 0.1f;

    private CellFactory _cellFactory;

    public void Init()
    {
        _cellFactory = GetComponent<CellFactory>(); 
    }

    public Cell[,,] Create(GridData data, Grid gridParent,  Vector3 center)
    {
        Cell[,,] grid = new Cell[data.Width, data.Length, data.Height];
        Sequence sequence = DOTween.Sequence();

        for (int x = 0; x < data.Width; x++)
        {
            for (int z = 0; z < data.Length; z++)
            {
                for (int y = 0; y < data.Height; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, z);
                    Cell cell = _cellFactory.Create(data.CellPrefab, position, gridParent);

                    Vector3 randomStartPosition = center + UnityEngine.Random.onUnitSphere * data.CellSize * 10;
                    cell.transform.position = randomStartPosition;

                    Block block = Instantiate(data.BlockPrefab, cell.transform);
                    block.SetCurrentCell(cell);
                    block.Init();
                    cell.SetOccupy(block);
                    grid[x, y, z] = cell;

                    Vector3 targetPosition = new Vector3(x * data.CellSize, y * data.CellSize, z * data.CellSize);
                    float delay = _delay * (x + y + z);

                    sequence.Insert(delay, cell.transform.DOMove(targetPosition, _spawnDuration).SetEase(Ease.OutBack));
                    sequence.Insert(delay, cell.transform.DORotate(Vector3.one * 360, _spawnDuration, RotateMode.FastBeyond360));

                    block.TrailRenderer.enabled = true;
                }
            }
        }

        return grid;
    }
}
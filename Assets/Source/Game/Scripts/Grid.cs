using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private GridData _data;

    private Cell[,] _grid;

    private void Awake() // разделить на фабрику
    {
        CreateGrid();
    }

    public Cell GetCell(Vector3Int position)
    {
        if (position.x >= 0 && position.x < _data.Width && position.z >= 0 && position.z < _data.Height)
        {
            return _grid[position.x, position.z];
        }
        return null;
    }

    private void CreateGrid()
    {
        _grid = new Cell[_data.Width, _data.Height];
        int blockCount = _data.BlockCount;

        for (int x = 0; x < _data.Width; x++)
        {
            for (int z = 0; z < _data.Height; z++)
            {
                GameObject cellObj = new GameObject($"Cell_{x}_{z}"); // заменить
                Cell cell = cellObj.AddComponent<Cell>();

                Vector3Int position = new Vector3Int(x, 0, z);
                cell.Initialize(position, this);
                cellObj.transform.position = new Vector3(x * _data.CellSize, 0, z * _data.CellSize);
                _grid[x, z] = cell;

                if (blockCount > 0)
                {
                    Block block = Instantiate(_data.Prefab, cellObj.transform);
                    block.SetCurrentCell(cell);
                    cell.SetOccupy(block); 

                    blockCount--;
                }
            }
        }
    }
}

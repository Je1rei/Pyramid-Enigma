using UnityEngine;

public class Cell : MonoBehaviour
{
    private Grid _grid;

    public Vector3Int Position { get; private set; }
    public Block Occupied { get; private set; }

    public void Init(Vector3Int position, Grid grid)
    {
        Position = position;
        _grid = grid;
        transform.SetParent(grid.transform);
    }

    public Grid GetGrid()
    {
        return _grid;
    }

    public bool IsOccupied()
    {
        return Occupied != null;
    }

    public void SetOccupy(Block block)
    {
        Occupied = block;
        block.transform.SetParent(transform);
        block.SetCurrentCell(this);
    }

    public void SetFree()
    {
        Occupied.SetCurrentCell(null);
        Occupied = null;
    }
}

using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector3Int Position { get; private set; }
    public Block Occupied { get; private set; }

    private Grid _grid;

    public void Initialize(Vector3Int position, Grid grid)
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
        block.SetCurrentCell(this);
    }

    public void SetFree()
    {
        Occupied = null;
    }
}

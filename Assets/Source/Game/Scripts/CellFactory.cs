using UnityEngine;

public class CellFactory : MonoBehaviour
{
    public Cell Create(Cell prefab, Vector3Int position, Grid grid)
    {
        Cell cell = Instantiate(prefab);
        cell.Initialize(position, grid);

        return cell;
    }
}

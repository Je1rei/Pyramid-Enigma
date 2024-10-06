using UnityEngine;

[CreateAssetMenu(fileName = "GridData", menuName = "ScriptableObjects/GridData", order = 1)]
public class GridData : ScriptableObject
{
    public Block BlockPrefab;
    public Cell CellPrefab;

    public int Width = 2;
    public int Height = 2;
    public int Length = 2;

    public float CellSize = 1f;
    public int BlockCount = 2;
}
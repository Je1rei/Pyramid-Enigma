using UnityEngine;

[CreateAssetMenu(fileName = "GridData", menuName ="ScriptableObjects/GridData", order = 1)] 
public class GridData: ScriptableObject
{
    public int Width = 2;
    public int Height = 2;
    public float CellSize = 1f;
    public Block Prefab;
    public int BlockCount = 2;
}
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GridData", menuName = "ScriptableObjects/GridData", order = 2)]
public class GridData : ScriptableObject
{
    [NonSerialized] public float CellSize = 1f;
    public Block BlockPrefab;
    public Cell CellPrefab;

    public int Width = 2;
    public int Height = 2;
    public int Length = 2;
}
using System;
using UnityEngine;

namespace Source.Game.Scripts
{
    [CreateAssetMenu(fileName = "GridData", menuName = "ScriptableObjects/GridData", order = 2)]
    public class GridData : ScriptableObject
    {
        [NonSerialized] public float CellSize = 1f;
        public Grid GridPrefab;
        public Block BlockPrefab;
        public Cell CellPrefab;

        public int Width = 2;
        public int Height = 2;
        public int Length = 2;
        public PaletteData PaletteData;
    }
}

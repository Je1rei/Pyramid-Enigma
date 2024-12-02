using UnityEngine;

[CreateAssetMenu(fileName = "PaletteData", menuName = "ScriptableObjects/PaletteData", order = 3)]
public class PaletteData : ScriptableObject
{
    public Color[] Palette;
}
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public bool IsOpen;
    public GridData GridData;
    public Treasure RewardedPrefab;
    public int TimeLimit;
}
﻿using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public int ID;
    public GridData GridData;
    public Treasure RewardedPrefab;
    public int TimeLimit;
    public Background Background;
}
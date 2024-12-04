using System;
using UnityEngine;
using YG;

public class Wallet : MonoBehaviour, IService
{
    private int _score;

    public int Score => _score;

    public event Action<int> Changed;

    public void Init()
    {
        _score = YG2.saves.score;
        Changed?.Invoke(_score);
    }

    public void Increase(int value)
    {
        _score += value;
        Changed?.Invoke(_score);
        YG2.saves.score = _score;

        YG2.SetLeaderboard(nameLB: "Score", score: YG2.saves.score);
        YG2.SaveProgress();
    }
}

namespace YG
{
    public partial class SavesYG
    {
        public int score = 0;
    }
}
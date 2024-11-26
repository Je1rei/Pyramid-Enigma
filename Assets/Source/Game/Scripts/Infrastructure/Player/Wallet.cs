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

    public void Increase()
    {
        YG2.saves.score += 1;
        _score++;
        Changed?.Invoke(_score);

        YG2.SetLeaderboard(nameLB: "ScoreLeaderboard", score: YG2.saves.score);
        YG2.SaveProgress();
    }

    //public void Decrease()
    //{
    //    if (_score <= 0)
    //    {
    //        Debug.LogError($"Score is {_score}");
    //    }
    //    else
    //    {
    //        Changed?.Invoke(_score);
    //        _score--;
    //        YG2.SaveProgress();
    //    }
    //}
}

namespace YG
{
    public partial class SavesYG
    {
        public int score = 0;
    }
}
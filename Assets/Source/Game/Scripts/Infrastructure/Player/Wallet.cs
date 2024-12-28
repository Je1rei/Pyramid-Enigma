using System;
using YG;

public class Wallet : IService
{
    private int _score;
    
    public event Action<int> ScoreChanged;
    
    public int Score => _score;

    public void Init()
    {
        _score = YG2.saves.Score;
        ScoreChanged?.Invoke(_score);
    }

    public void IncreaseScore(int value)
    {
        _score += value;
        ScoreChanged?.Invoke(_score);
        YG2.saves.Score = _score;

        YG2.SetLeaderboard(nameLB: "Score", score: YG2.saves.Score);
        YG2.SaveProgress();
    }
}

namespace YG
{
    public partial class SavesYG
    {
        public int Score = 0;
    }
}

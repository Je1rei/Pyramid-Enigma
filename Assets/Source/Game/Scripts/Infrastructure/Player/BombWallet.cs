using System;
using YG;

public class BombWallet :IService
{
    private int _decreaseValue = 1;
    private int _value;

    public int Value => _value;

    public event Action<int> CountChanged;

    public void Init()
    {
        _value = YG2.saves.Bombs;
        CountChanged?.Invoke(_value);
    }

    public void IncreaseScore(int rewardValue = 1)
    {
        _value += rewardValue;
        CountChanged?.Invoke(_value);

        YG2.saves.Bombs = _value;
        YG2.SaveProgress();
    }

    public void DecreaseScore()
    {
        if (_value > 0)
        {
            _value -= _decreaseValue;
            CountChanged?.Invoke(_value);

            YG2.saves.Bombs = _value;
            YG2.SaveProgress();
        }
    }
}

namespace YG
{
    public partial class SavesYG
    {
        public int Bombs = 10;
    }
}
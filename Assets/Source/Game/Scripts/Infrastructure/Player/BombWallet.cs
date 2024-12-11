using System;
using UnityEngine;
using YG;

public class BombWallet : MonoBehaviour, IService
{
    [SerializeField] private int _decreaseValue = 1;
    private int _bombs;

    public int Bombs => _bombs;

    public event Action<int> CountChanged;

    public void Init()
    {
        _bombs = YG2.saves.Bombs;
        CountChanged?.Invoke(_bombs);
    }

    public void IncreaseScore(int rewardValue = 5)
    {
        _bombs += rewardValue;
        CountChanged?.Invoke(_bombs);

        YG2.saves.Bombs = _bombs;
        YG2.SaveProgress();
    }

    public void DecreaseScore()
    {
        if (_bombs > 0)
        {
            _bombs -= _decreaseValue;
            CountChanged?.Invoke(_bombs);

            YG2.saves.Bombs = _bombs;
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
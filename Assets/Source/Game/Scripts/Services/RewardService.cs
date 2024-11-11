using System;
using UnityEngine;

public class RewardService : MonoBehaviour, IService
{
    private Grid _grid;
    private Wallet _wallet;
    private TimerService _timerService;
    private TreasureFactory _factory;
    private Treasure _prefab;

    public event Action Rewarded;
    public event Action Losed;

    private void OnDisable()
    {
        if (_grid != null)
            _grid.BlocksMoved -= Reward;

        if (_timerService != null)
            _timerService.Ended -= Lose;
    }

    public void Init(Grid grid, TreasureFactory factory, Treasure prefab)
    {
        _timerService = ServiceLocator.Current.Get<TimerService>();
        _wallet = ServiceLocator.Current.Get<Wallet>();

        _grid = grid;
        _factory = factory;
        _prefab = prefab;

        _grid.BlocksMoved += Reward;
        _timerService.Ended += Lose;
    }

    public void Reward()
    {
        Treasure treasure = _factory.Create(_prefab);
        treasure.Init();

        _timerService.Deactivate();
        _wallet.Increase();
        Rewarded?.Invoke();
    }

    public void Lose()
    {
        _timerService.Deactivate();
        Losed?.Invoke();
    }
}

using System;
using UnityEngine;

public class RewardService : MonoBehaviour, IService
{
    private Grid _grid;
    private Wallet _wallet;

    private LevelService _levelService;
    private TimerService _timerService;
    private Treasure _treasure;

    public event Action Rewarded;
    public event Action Losed;

    public void Init(Grid grid,Treasure prefab)
    {
        _timerService = ServiceLocator.Current.Get<TimerService>();
        _wallet = ServiceLocator.Current.Get<Wallet>();
        _levelService = ServiceLocator.Current.Get<LevelService>();

        _grid = grid;
        _treasure = prefab;

        _grid.AllBlocksMoved += Reward;
        _timerService.Ended += Lose;
    }

    public void Reward()
    {
        _levelService.Complete();
        _timerService.Deactivate();
        _wallet.Increase(_treasure.Value);
        Rewarded?.Invoke();

        UnSubscribe();
    }

    public void Lose()
    {
        _timerService.Deactivate();
        Losed?.Invoke();
        UnSubscribe();
    }

    private void UnSubscribe()
    {
        if (_grid != null)
            _grid.AllBlocksMoved -= Reward;

        if (_timerService != null)
            _timerService.Ended -= Lose;
    }
}

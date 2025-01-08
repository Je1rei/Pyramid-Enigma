using System;

public class RewardService : IService
{
    private Grid _grid;
    private Wallet _wallet;
    private BombWallet _bombWallet;

    private LevelService _levelService;
    private TimerService _timerService;
    private Treasure _treasure;

    public event Action Rewarded;
    public event Action Losed;

    public int Value => _treasure.Value;

    public void Init(Grid grid, Treasure prefab)
    {
        _timerService = ServiceLocator.Current.Get<TimerService>();
        _wallet = ServiceLocator.Current.Get<Wallet>();
        _levelService = ServiceLocator.Current.Get<LevelService>();
        _bombWallet = ServiceLocator.Current.Get<BombWallet>();

        _grid = grid;
        _treasure = prefab;

        _grid.BlocksReleased += Reward;
        _timerService.Ended += Lose;
    }

    public void Reward()
    {
        _levelService.Complete();
        _timerService.Deactivate();
        _wallet.IncreaseScore(_treasure.Value);
        _bombWallet.IncreaseScore();
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
        {
            _grid.BlocksReleased -= Reward;
        }

        if (_timerService != null)
        {
            _timerService.Ended -= Lose;
        }
    }
}
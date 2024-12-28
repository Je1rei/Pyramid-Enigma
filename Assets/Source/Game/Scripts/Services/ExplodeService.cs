using System;

public class ExplodeService : IService
{
    private BombWallet _bombWallet;
    private bool _isActive;

    public event Action BombIsEmpty;

    public bool IsActive => _isActive;

    public void Init()
    {
        _bombWallet = ServiceLocator.Current.Get<BombWallet>();
        _isActive = false;
        _bombWallet.CountChanged += FailActivate;
    }

    public void Deactivate() => _isActive = false;

    public void Activate()
    {
        if (_bombWallet.Value > 0)
        {
            _isActive = true;
        }
        else
        {
            FailActivate(_bombWallet.Value);
        }
    }

    private void FailActivate(int value)
    {
        if (value <= 0)
        {
            BombIsEmpty?.Invoke();
        }
    }
}
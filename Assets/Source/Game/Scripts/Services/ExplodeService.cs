using System;
using UnityEngine;
using YG;

public class ExplodeService : MonoBehaviour, IService
{
    private bool _isActive;
    private BombWallet _bombWallet;

    public bool IsActive => _isActive;

    public event Action BombIsEmpty;

    public void Init()
    {
        _bombWallet = ServiceLocator.Current.Get<BombWallet>();
        _isActive = false;
        _bombWallet.CountChanged += FailActivate;
    }

    public void Deactivate() => _isActive = false;

    public void Activate()
    {
        if (_bombWallet.Bombs > 0)
            _isActive = true;
        else
            FailActivate(_bombWallet.Bombs);
    }

    private void FailActivate(int value)
    {
        if(value <= 0)
            BombIsEmpty?.Invoke();
    }
}

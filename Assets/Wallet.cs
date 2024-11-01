using UnityEngine;

public class Wallet : MonoBehaviour, IWalletService
{
    private int _score;

    public int Score => _score;

    public void Increase()
    {
        _score++;
    }

    public void Decrease()
    {
        if (_score <= 0)
            Debug.LogError($"Score is {_score}");
        else
            _score--;
    }
}

public interface IWalletService : IService
{
    public void Increase();
    public void Decrease();
}

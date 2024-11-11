using System;
using UnityEngine;

public class Wallet : MonoBehaviour, IService
{
    private int _score;

    public int Score => _score;

    public event Action<int> Changed;

    public void Increase()
    {
        _score++;
        Changed?.Invoke(_score);
    }

    public void Decrease()
    {
        if (_score <= 0)
        {
            Debug.LogError($"Score is {_score}");
        }
        else
        {
            Changed?.Invoke(_score);
            _score--;
        }
    }
}

using System;
using UnityEngine;

public class TimerService : IService
{
    private int _timeLimit;
    private float _timeRemaining;
    private bool _isActive;

    public event Action<int> Changed;
    public event Action Ended;

    public void Deactivate() => _isActive = false;

    public void Init(int timeLimit)
    {
        _isActive = false;
        _timeLimit = timeLimit;

        Setup();
    }

    public void Tick()
    {
        if (_isActive)
            UpdateTimer();
    }

    private void Setup()
    {
        _timeRemaining = _timeLimit;
        _isActive = true;
    }

    private void UpdateTimer()
    {
        _timeRemaining -= Time.deltaTime;

        if (_timeRemaining <= 0)
        {
            _timeRemaining = 0;
            _isActive = false;
            Ended?.Invoke();
        }

        Changed?.Invoke((int)_timeRemaining);
    }
}
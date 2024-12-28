using UnityEngine;
using DG.Tweening;

public class InputPause : MonoBehaviour, IService
{
    private Sequence _sequence;

    public bool CanInput { get; private set; }

    public void Init()
    {
        CanInput = false;
    }

    public void ActivateInputCooldown(float duration = 0.5F)
    {
        StartCooldown(duration);
    }

    public void ActivateInput() => CanInput = true;

    public void DeactivateInput() => CanInput = false;

    public void StartCooldown(float duration)
    {
        if (CanInput == false)
        {
            return;
        }

        _sequence = DOTween.Sequence();
        CanInput = false;

        _sequence.PrependInterval(duration).OnComplete(() => { CanInput = true; });
    }
}
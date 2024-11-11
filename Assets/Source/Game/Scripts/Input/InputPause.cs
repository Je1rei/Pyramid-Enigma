using UnityEngine;
using DG.Tweening;

public class InputPause : MonoBehaviour, IService
{
    public bool CanInput { get; private set; }

    public void Init()
    {
        CanInput = true;
    }

    public void ActivateInputCooldown(float duration) 
    {
        CanInput = true;
        StartCooldown(duration); 
    }

    public void DeactivateInput() => CanInput = false;

    public void StartCooldown(float duration)
    {
        if (CanInput == false)
            return;

        CanInput = false;

        DOVirtual.DelayedCall(duration, () =>
        {
            CanInput = true;
        });
    }
}
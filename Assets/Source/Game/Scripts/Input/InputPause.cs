using UnityEngine;
using DG.Tweening;

public class InputPause : MonoBehaviour, IService
{
    public bool CanInput { get; private set; } = true;

    public void ActivateInputCooldown() => StartCooldown(3f);

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
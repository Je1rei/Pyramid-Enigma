using DG.Tweening;
using UnityEngine;

namespace Source.Game.Scripts
{
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

            Sequence sequence = DOTween.Sequence();
            CanInput = false;

            sequence.PrependInterval(duration).OnComplete(() => { CanInput = true; });
        }
    }
}
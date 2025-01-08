using System;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Game.Scripts
{
    public abstract class UIPanel : MonoBehaviour
    {
        private AudioService _audioService;

        protected void SetAudioService() => _audioService = ServiceLocator.Current.Get<AudioService>();

        public void Show()
        {
            if (this != null)
            {
                gameObject.SetActive(true);
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        protected void AddButtonListener(Button button, Action onClickAction)
        {
            button.onClick.AddListener(() =>
            {
                _audioService.PlaySound();
                onClickAction?.Invoke();
            });
        }
    }
}
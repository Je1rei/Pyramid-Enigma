using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Game.Scripts
{
    public class MainMenuPanel : UIPanel
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private UIPanel _levels;
        [SerializeField] private UIPanel _settings;

        private void OnEnable()
        {
            SetAudioService();

            AddButtonListener(_playButton, OnClickPlay);
            AddButtonListener(_settingsButton, OnClickSettings);
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveAllListeners();
            _settingsButton.onClick.RemoveAllListeners();
        }

        private void OnClickPlay()
        {
            _levels.Show();
            Hide();
        }

        private void OnClickSettings()
        {
            _settings.Show();
            Hide();
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace Source.Game.Scripts
{
    public class LeaderboardPanel : UIPanel
    {
        [SerializeField] private SettingsPanel _settingsPanel;
        [SerializeField] private Button _backButton;

        private void OnEnable()
        {
            SetAudioService();
            AddButtonListener(_backButton, OnClickBack);
        }

        private void OnDisable()
        {
            _backButton.onClick.RemoveAllListeners();
        }

        private void OnClickBack()
        {
            _settingsPanel.Show();
            Hide();
        }
    }
}
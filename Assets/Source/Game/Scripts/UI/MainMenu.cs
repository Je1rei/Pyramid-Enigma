using UnityEngine;
using UnityEngine.UI;

public class MainMenu : UIPanel
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private UIPanel _levels;
    [SerializeField] private UIPanel _settings;

    private void OnEnable()
    {
        _playButton.onClick.AddListener(OnClickPlay);
        _settingsButton.onClick.AddListener(OnClickSettings);
    }

    private void OnDisable()
    {
        _playButton.onClick.RemoveAllListeners();
        _settingsButton.onClick.RemoveAllListeners();
    }

    private void OnClickPlay()
    {
        Hide();
        _levels.Show();
    }

    private void OnClickSettings()
    {
        Hide();
        _settings.Show();
    }
}
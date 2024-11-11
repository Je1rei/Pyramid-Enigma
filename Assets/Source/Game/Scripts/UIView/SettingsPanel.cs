using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : UIPanel
{
    [SerializeField] private MainMenu _mainMenu;
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
        Hide();
        _mainMenu.Show();
    }
}
using UnityEngine;
using UnityEngine.UI;
using YG;

public class AuthorizationPanel: UIPanel
{
    [SerializeField] private SettingsPanel _settingsPanel;
    [SerializeField] private Button _agreeButton;
    [SerializeField] private Button _backButton;

    private void OnEnable()
    {
        SetAudioService();

        AddButtonListener(_agreeButton, OnClickAgree);
        AddButtonListener(_backButton, OnClickBack);
    }

    private void OnDisable()
    {
        _agreeButton.onClick.RemoveAllListeners();
        _backButton.onClick.RemoveAllListeners();
    }

    private void OnClickAgree()
    {
        YG2.OpenAuthDialog();
        _settingsPanel.Show();
        Hide();
    }

    private void OnClickBack()
    {
        _settingsPanel.Show();
        Hide();
    }
}

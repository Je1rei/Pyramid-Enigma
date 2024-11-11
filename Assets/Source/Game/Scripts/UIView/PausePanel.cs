using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePanel : UIPanel
{
    [SerializeField] private GameplayPanel _gameplayPanel;
    [SerializeField] private Button _backToMenuButton;
    [SerializeField] private Button _continueButton;

    private void OnEnable()
    {
        SetAudioService();
        AddButtonListener(_continueButton, OnClickUnPause);
        AddButtonListener(_backToMenuButton, OnClickBackToMenu);
    }

    private void OnDisable()
    {
        _continueButton.onClick.RemoveAllListeners();
        _backToMenuButton.onClick.RemoveAllListeners();
    }

    public void Pause()
    {
        Show();
        Time.timeScale = 0f;
    }

    public void OnClickUnPause()
    {
        Time.timeScale = 1f;
        Hide();
        _gameplayPanel.UnPause();
    }

    private void OnClickBackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}

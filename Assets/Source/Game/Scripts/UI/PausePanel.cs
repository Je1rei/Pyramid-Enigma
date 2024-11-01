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
        _continueButton.onClick.AddListener(UnPause);
        _backToMenuButton.onClick.AddListener(OnClickBackToMenu);
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

    public void UnPause()
    {
        Hide();
        _gameplayPanel.UnPause();
    }

    private void OnClickBackToMenu()
    {
        SceneManager.LoadScene(1);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayPanel : UIPanel
{
    [SerializeField] private PausePanel _pausePanel;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _resetButton;

    private InputPause _inputPauser;

    private void OnEnable()
    {
        _backButton.onClick.AddListener(Pause);
        _resetButton.onClick.AddListener(OnClickReset);
    }

    private void OnDisable()
    {
        _backButton.onClick.RemoveAllListeners();
        _resetButton.onClick.RemoveAllListeners();
    }

    public void Init()
    {
        _inputPauser = ServiceLocator.Current.Get<InputPause>();
    }

    public void Pause()
    {
        _inputPauser.DeactivateInput();

        Hide();
        _pausePanel.Pause();
    }

    public void UnPause()
    {
        Show();
        Time.timeScale = 1f;

        _inputPauser.ActivateInputCooldown();
    }

    private void OnClickReset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().path);
    }
}
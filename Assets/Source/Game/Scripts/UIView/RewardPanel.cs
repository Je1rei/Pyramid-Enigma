using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RewardPanel : UIPanel
{
    [SerializeField] private GameplayPanel _gameplayPanel;
    [SerializeField] private Button _backToMenuButton;

    private RewardService _rewardService;

    private void OnEnable()
    {
        AddButtonListener(_backToMenuButton, OnClickBackToMenu);
    }

    private void OnDisable()
    {
        _backToMenuButton.onClick.RemoveAllListeners();
        _rewardService.Rewarded -= Reward;
    }

    public void Init()
    {
        SetAudioService();
        _rewardService = ServiceLocator.Current.Get<RewardService>();

        _rewardService.Rewarded += Reward;
    }

    private void Reward()
    {
        if (this != null)
        {
            Show();
            _gameplayPanel.Hide();
        }
    }

    private void OnClickBackToMenu()
    {
        SceneManager.LoadScene(1);
    }
}
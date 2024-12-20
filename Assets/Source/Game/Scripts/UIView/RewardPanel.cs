using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class RewardPanel : UIPanel
{
    [SerializeField] private ReceivedPrizePanel _receivedPrizePanel;
    [SerializeField] private GameplayPanel _gameplayPanel;
    [SerializeField] private Button _backToMenuButton;
    [SerializeField] private Button _rewardAdButton;
    [SerializeField] private Button _nextLevelButton;

    private RewardService _rewardService;
    
    private void OnEnable()
    {
        AddButtonListener(_backToMenuButton, OnClickBackToMenu);
        AddButtonListener(_rewardAdButton, OnClickAdReward);
        AddButtonListener(_nextLevelButton, OnClickNextLevel);
    }

    private void OnDisable()
    {
        _backToMenuButton.onClick.RemoveAllListeners();
        _nextLevelButton.onClick.RemoveAllListeners();

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

    private void OnClickAdReward()
    {
        Hide();
        _receivedPrizePanel.RewardAd();
    }
    
    private void OnClickBackToMenu()
    {
        SceneManager.LoadScene(1);
    }

    private void OnClickNextLevel()
    {
        LevelService levelService = ServiceLocator.Current.Get<LevelService>();
        int tempIndex = levelService.Index;
        LevelData leveldata = levelService.Load(tempIndex);

        if (leveldata != null)
        {
            YG2.InterstitialAdvShow();

            SceneManager.LoadScene(2);
        }
        else
        {
            _nextLevelButton.gameObject.SetActive(false);
        }
    }
}



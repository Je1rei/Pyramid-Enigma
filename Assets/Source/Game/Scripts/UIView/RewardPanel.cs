using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

namespace Source.Game.Scripts
{
    public class RewardPanel : UIPanel
    {
        [SerializeField] private ReceivedPrizePanel _receivedPrizePanel;
        [SerializeField] private GameplayPanel _gameplayPanel;
        [SerializeField] private Button _backToMenuButton;
        [SerializeField] private Button _rewardAdButton;
        [SerializeField] private Button _nextLevelButton;

        private RewardService _rewardService;
        private SceneLoaderService _loaderService;
        
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
            _loaderService = ServiceLocator.Current.Get<SceneLoaderService>();
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
            SceneManager.LoadScene(_loaderService.MainMenuScene);
        }

        private void OnClickNextLevel()
        {
            LevelService levelService = ServiceLocator.Current.Get<LevelService>();
            int tempIndex = levelService.ID;
            LevelData leveldata = levelService.Load(tempIndex);

            if (leveldata != null)
            {
                YG2.InterstitialAdvShow();

                SceneManager.LoadScene(_loaderService.GamePlayScene);
            }
            else
            {
                _nextLevelButton.gameObject.SetActive(false);
            }
        }
    }
}
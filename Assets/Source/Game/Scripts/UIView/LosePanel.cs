using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Source.Game.Scripts
{
    public class LosePanel : UIPanel
    {
        [SerializeField] private GameplayPanel _gameplayPanel;
        [SerializeField] private Button _backToMenuButton;
        [SerializeField] private Button _restartButton;

        private RewardService _rewardService;
        private InputPause _inputPauser;
        private SceneLoaderService _loaderService;
        
        private void OnEnable()
        {
            AddButtonListener(_restartButton, OnClickReset);
            AddButtonListener(_backToMenuButton, OnClickBackToMenu);
        }

        private void OnDisable()
        {
            _restartButton.onClick.RemoveAllListeners();
            _backToMenuButton.onClick.RemoveAllListeners();
            _rewardService.Losed -= Lose;
        }

        public void Init()
        {
            SetAudioService();
            _loaderService = ServiceLocator.Current.Get<SceneLoaderService>();
            _inputPauser = ServiceLocator.Current.Get<InputPause>();
            _rewardService = ServiceLocator.Current.Get<RewardService>();

            _rewardService.Losed += Lose;
        }

        private void Lose()
        {
            if (this != null)
            {
                Show();
                _inputPauser.DeactivateInput();
                _gameplayPanel.Hide();
            }
        }

        private void OnClickReset()
        {
            SceneManager.LoadScene(_loaderService.GamePlayScene);
        }

        private void OnClickBackToMenu()
        {
            SceneManager.LoadScene(_loaderService.MainMenuScene);
        }
    }
}
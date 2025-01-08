using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Source.Game.Scripts
{
    public class PausePanel : UIPanel
    {
        [SerializeField] private GameplayPanel _gameplayPanel;
        [SerializeField] private Button _backToMenuButton;
        [SerializeField] private Button _continueButton;

        private SceneLoaderService _loaderService;
        
        private void OnEnable()
        {
            SetAudioService();
            AddButtonListener(_continueButton, OnClickUnPause);
            AddButtonListener(_backToMenuButton, OnClickBackToMenu);

            _loaderService = ServiceLocator.Current.Get<SceneLoaderService>();
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
            AudioListener.pause = true;
        }

        public void OnClickUnPause()
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
            Hide();
            _gameplayPanel.UnPause();
        }

        private void OnClickBackToMenu()
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
            SceneManager.LoadScene(_loaderService.MainMenuScene);
        }
    }
}

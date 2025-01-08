using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Source.Game.Scripts
{
    public class GameplayPanel : UIPanel
    {
        [SerializeField] private RectTransform _parentBackground;
        [SerializeField] private TutorialPanel _tutorialPanel;
        [SerializeField] private PausePanel _pausePanel;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _resetButton;
        [SerializeField] private TMP_Text _timerText;
    
        private InputPause _inputPauser;
        private TimerService _timerService;
        private SceneLoaderService _loaderService;
        
        private void OnEnable()
        {
            Time.timeScale = 1f;

            AddButtonListener(_backButton, Pause);
            AddButtonListener(_resetButton, OnClickReset);

            if (_timerService != null)
            {
                _timerService.Changed += OnChangedTime;
            }
        }

        private void OnDisable()
        {
            _backButton.onClick.RemoveAllListeners();
            _resetButton.onClick.RemoveAllListeners();
            _timerService.Changed -= OnChangedTime;
        }

        public void Init()
        {
            _loaderService = ServiceLocator.Current.Get<SceneLoaderService>();
            _inputPauser = ServiceLocator.Current.Get<InputPause>();
            _timerService = ServiceLocator.Current.Get<TimerService>();
        
            SetAudioService();
            Show();
            _timerService.Changed += OnChangedTime;
            _tutorialPanel.Init();
            ServiceLocator.Current.Get<LevelService>().CreateBackground(_parentBackground);
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
            _inputPauser.ActivateInput();
        }

        private void OnClickReset()
        {
            SceneManager.LoadScene(_loaderService.GamePlayScene);
        }

        private void OnChangedTime(int value)
        {
            _timerText.text = value.ToString();
        }
    }
}
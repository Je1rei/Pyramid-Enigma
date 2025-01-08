using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

namespace Source.Game.Scripts
{
    public class LevelsPanel : UIPanel
    {
        [SerializeField] private MainMenuPanel _mainMenu;
        [SerializeField] private Level[] _levels;

        [SerializeField] private LevelPage[] _levelPages;
        [SerializeField] private Button[] _pageButtons;
        [SerializeField] private Button _backButton;

        private LevelService _levelService;
        private SceneLoaderService _loaderService;
        

        private void OnEnable()
        {
            _loaderService = ServiceLocator.Current.Get<SceneLoaderService>();
            _levelService = ServiceLocator.Current.Get<LevelService>();
            SetAudioService();

            for (int i = 0; i < _levels.Length; i++)
            {
                LevelData levelData = _levelService.Load(i);

                if (levelData != null && YG2.saves.OpenedLevels.Contains(levelData.ID))
                {
                    _levels[i].SetUnlock();
                }
                else
                {
                    _levels[i].SetLock();
                }

                int levelIndex = i;
                AddButtonListener(_levels[i].Button, () => OnClickLevel(levelIndex));
            }

            for (int i = 0; i < _pageButtons.GetLength(0); i++)
            {
                int index = i;
                AddButtonListener(_pageButtons[index], () => SwitchPage(index));
            }

            AddButtonListener(_backButton, OnClickBack);
            SwitchPage(0);
        }

        private void OnDisable()
        {
            foreach (Level level in _levels)
                level.Button.onClick.RemoveAllListeners();

            _backButton.onClick.RemoveAllListeners();
        }

        private void OnClickLevel(int index)
        {
            LevelData levelData = _levelService.Load(index);

            if (levelData != null)
            {
                Hide();
                SceneManager.LoadScene(_loaderService.GamePlayScene);
            }
        }

        private void OnClickBack()
        {
            _mainMenu.Show();
            Hide();
        }

        private void SwitchPage(int index)
        {
            foreach (LevelPage page in _levelPages)
                page.Hide();

            if (index >= 0 && index < _levelPages.GetLength(0))
            {
                _levelPages[index].Show();
            }

            for (int i = 0; i < _pageButtons.GetLength(0); i++)
                _pageButtons[i].interactable = i != index;
        }
    }
}
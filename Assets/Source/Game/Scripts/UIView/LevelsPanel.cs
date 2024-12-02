using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class LevelsPanel : UIPanel
{
    [SerializeField] private MainMenuPanel _mainMenu;
    [SerializeField] private Level[] _levels;
    [SerializeField] private Button _backButton;

    private LevelService _levelService;

    private void OnEnable()
    {
        _levelService = ServiceLocator.Current.Get<LevelService>();
        SetAudioService();

        for (int i = 0; i < _levels.Length; i++)
        {
            if (YG2.saves.OpenLevels[i])
            {
                Debug.Log(i);
                _levels[i].SetUnlock();
            }
            else
            {
                _levels[i].SetLock();
            }

            int levelIndex = i;
            AddButtonListener(_levels[i].Button, () => OnClickLevel(levelIndex));
        }

        AddButtonListener(_backButton, OnClickBack);
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
            SetAudioService();
            Hide();
            SceneManager.LoadScene(2);
        }
    }

    private void OnClickBack()
    {
        SetAudioService();
        _mainMenu.Show();
        Hide();
    }
}

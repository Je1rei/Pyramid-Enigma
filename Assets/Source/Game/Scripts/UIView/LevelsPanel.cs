using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsPanel : UIPanel
{
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private Button[] _levelButtons;
    [SerializeField] private Button _backButton;

    private LevelService _levelService;

    private void OnEnable()
    {
        _levelService = ServiceLocator.Current.Get<LevelService>();
        SetAudioService();

        for (int i = 0; i < _levelButtons.Length; i++)
        {
            int levelIndex = i;
            AddButtonListener(_levelButtons[i], () => OnClickLevel(levelIndex));
        }

        AddButtonListener(_backButton, OnClickBack);
    }

    private void OnDisable()
    {
        foreach (Button button in _levelButtons)
            button.onClick.RemoveAllListeners();

        _backButton.onClick.RemoveAllListeners();
    }

    private void OnClickLevel(int index)
    {
        LevelData levelData = _levelService.LoadLevel(index);

        if(levelData != null)
        {
            SetAudioService();
            Hide();
            SceneManager.LoadScene(2);  
        }
    }
    
    private void OnClickBack()
    {
        SetAudioService();
        Hide();
        _mainMenu.Show();
    }
}

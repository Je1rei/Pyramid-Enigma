using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsPanel : UIPanel
{
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private Button[] _levelButtons;
    [SerializeField] private Button _backButton;

    private void OnEnable()
    {
        foreach (Button button in _levelButtons)
            button.onClick.AddListener(OnClickLevel);

        _backButton.onClick.AddListener(OnClickBack);
    }

    private void OnDisable()
    {
        foreach (Button button in _levelButtons)
            button.onClick.RemoveAllListeners();

        _backButton.onClick.RemoveAllListeners();
    }

    private void OnClickLevel()
    {
        Hide();
        SceneManager.LoadScene(2); // добавить логику выбора уровня
    }
    
    private void OnClickBack()
    {
        Hide();
        _mainMenu.Show();
    }

#if UNITY_EDITOR
    [ContextMenu("Refresh Child Array")]
    private void RefreshChildArray()
    {
        int levelsCount = transform.childCount;
        _levelButtons = new Button[levelsCount];

        for (int i = 0; i < levelsCount; i++)
            _levelButtons[i] = transform.GetChild(i).GetComponent<Button>();
    }
#endif
}

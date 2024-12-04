using UnityEngine;

public class TutorialPanel : UIPanel
{
    [SerializeField] private TutorialStepPanel[] _tutorialPanels;

    private TutorialService _service;
    private SwipeInputHandler _swipeInputHandler;

    private int _currentIndex;

    private void OnDisable()
    {
        _swipeInputHandler.Clicked -= ShowNextTutorial;
        _service.Deactivate();
    }

    public void Init()
    {
        _swipeInputHandler = ServiceLocator.Current.Get<SwipeInputHandler>();
        _service = ServiceLocator.Current.Get<TutorialService>();   

        if (_service.IsActive)
        {
            Show();
            SetupSequenceTutorials();
            _swipeInputHandler.Clicked += ShowNextTutorial;
        }
    }

    private void SetupSequenceTutorials()
    {
        foreach (var panel in _tutorialPanels)
        {
            panel.gameObject.SetActive(false);
        }

        if (_tutorialPanels.Length > 0)
        {
            _currentIndex = 0;
            _tutorialPanels[_currentIndex].gameObject.SetActive(true);
        }
    }

    private void ShowNextTutorial()
    {
        if (_currentIndex < _tutorialPanels.Length)
        {
            _tutorialPanels[_currentIndex].gameObject.SetActive(false);
            _currentIndex++;

            if (_currentIndex < _tutorialPanels.Length)
            {
                _tutorialPanels[_currentIndex].gameObject.SetActive(true);
            }
            else
            {
                CompleteTutorial();
            }
        }
    }

    private void CompleteTutorial()
    {
        _swipeInputHandler.Clicked -= ShowNextTutorial;
        _service.Deactivate();
    }
}
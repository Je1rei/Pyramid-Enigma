using UnityEngine;
using UnityEngine.SceneManagement;

public class CompositeRootGameplay : CompositeRoot
{
    [SerializeField] private Grid _grid;
    [SerializeField] private CameraMover _cameraMover;
    [SerializeField] private TreasureFactory _treasureFactory;
    [SerializeField] private AudioSource _uiAudioSource;

    [Header("UIPanels")]
    [SerializeField] private GameplayPanel _gameplayPanel;
    [SerializeField] private RewardPanel _rewardPanel;
    [SerializeField] private LosePanel _losePanel;

    private RewardService _rewardService;
    private Wallet _wallet;
    private TimerService _timerService;
    private LevelService _levelService;
    private InputPause _inputPauser;
    private SwipeInputHandler _swipeInputHandler;
    private AudioService _audioService;
    private ServiceLocator _serviceLocator;

    private void Awake()
    {
        _serviceLocator = ServiceLocator.Current;
        Compose();
    }

    private void Update()
    {
        if (_inputPauser.CanInput)
        {
            _swipeInputHandler.Tick();
        }

        _timerService.Tick();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        Compose();
    }

    public override void Compose()
    {
        _rewardService = _serviceLocator.Get<RewardService>();
        _wallet = _serviceLocator.Get<Wallet>();
        _levelService = _serviceLocator.Get<LevelService>();
        _audioService = _serviceLocator.Get<AudioService>();
        _inputPauser = _serviceLocator.Get<InputPause>();
        _swipeInputHandler = _serviceLocator.Get<SwipeInputHandler>();
        _timerService = _serviceLocator.Get<TimerService>();

        _audioService.Init(_uiAudioSource);
        _grid.Init(_levelService.Current.GridData);
        _timerService.Init(_levelService.Current.TimeLimit);

        _cameraMover.Init();
        _swipeInputHandler.Init(_grid);
        _inputPauser.Init();
        _rewardService.Init(_grid, _treasureFactory, _levelService.Current.RewardedPrefab);

        _rewardPanel.Init();
        _gameplayPanel.Init();
        _losePanel.Init();
    }
}
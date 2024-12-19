using UnityEngine;

public class CompositeRootGameplay : CompositeRoot
{
    [SerializeField] private Grid _grid;
    [SerializeField] private CameraMover _cameraMover;
    [SerializeField] private AudioSource _uiAudioSource;

    [Header("UIPanels")]
    [SerializeField] private GameplayPanel _gameplayPanel;
    [SerializeField] private RewardPanel _rewardPanel;
    [SerializeField] private LosePanel _losePanel;
    [SerializeField] private BombWalletView _bombWalletView;

    private ExplodeService _explosionService;
    private BombWallet _bombWallet;
    private RewardService _rewardService;
    private TimerService _timerService;
    private LevelService _levelService;
    private InputPause _inputPauser;
    private SwipeInputHandler _swipeInputHandler;
    private TutorialService _tutorialService;
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

    public override void Compose()
    {
        _explosionService = _serviceLocator.Get<ExplodeService>();
        _bombWallet = _serviceLocator.Get<BombWallet>();
        _rewardService = _serviceLocator.Get<RewardService>();
        _levelService = _serviceLocator.Get<LevelService>();
        _audioService = _serviceLocator.Get<AudioService>();
        _inputPauser = _serviceLocator.Get<InputPause>();
        _swipeInputHandler = _serviceLocator.Get<SwipeInputHandler>();
        _timerService = _serviceLocator.Get<TimerService>();
        _tutorialService = _serviceLocator.Get<TutorialService>();

        Initialize();
    }

    private void Initialize()
    {
        _explosionService.Init();
        _bombWallet.Init();
        _bombWalletView.Init();
        _audioService.Init(_uiAudioSource);
        _grid.Init(_levelService.Current.GridData);
        _timerService.Init(_levelService.Current.TimeLimit);

        _cameraMover.Init();
        _inputPauser.Init();
        _swipeInputHandler.Init(_grid);
        _rewardService.Init(_grid, _levelService.Current.RewardedPrefab);
        _tutorialService.Init();

        _rewardPanel.Init();
        _gameplayPanel.Init();
        _losePanel.Init();
    }
}
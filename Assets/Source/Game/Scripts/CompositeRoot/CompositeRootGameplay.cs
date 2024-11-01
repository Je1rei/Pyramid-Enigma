using UnityEngine;

public class CompositeRootGameplay : CompositeRoot
{
    [SerializeField] private Grid _grid;
    [SerializeField] private CameraMover _cameraMover;
    [SerializeField] private GameplayPanel _gameplayPanel;

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
            Debug.Log("falfla");
            _swipeInputHandler.Tick();
        }
    }

    public override void Compose()
    {
        _audioService = _serviceLocator.Get<AudioService>();
        _inputPauser = _serviceLocator.Get<InputPause>();
        _swipeInputHandler = _serviceLocator.Get<SwipeInputHandler>();

        _cameraMover.Init();
        _grid.Init();
        _swipeInputHandler.Init(_grid);
        _gameplayPanel.Init();
    }
}
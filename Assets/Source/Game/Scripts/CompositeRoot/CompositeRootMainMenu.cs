using UnityEngine;

public class CompositeRootMainMenu : CompositeRoot
{
    [SerializeField] private WalletView _walletView;
    [SerializeField] private AudioSource _uiAudioSource;

    private Wallet _wallet;
    private AudioService _audioService;
    private LevelService _levelService;

    private ServiceLocator _serviceLocator;

    private void Awake()
    {
        _serviceLocator = ServiceLocator.Current;
        Compose();
    }

    public override void Compose()
    {
        _wallet = _serviceLocator.Get<Wallet>();    
        _levelService = _serviceLocator.Get<LevelService>();
        _audioService = _serviceLocator.Get<AudioService>();

        _walletView.Init();
        _audioService.Init(_uiAudioSource);
    }
}
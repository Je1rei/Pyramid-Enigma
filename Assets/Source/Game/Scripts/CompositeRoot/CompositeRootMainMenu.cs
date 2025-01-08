using UnityEngine;
using YG;

public class CompositeRootMainMenu : CompositeRoot
{
    [SerializeField] private WalletView _walletView;
    [SerializeField] private AudioSource _uiAudioSource;

    private Wallet _wallet;
    private AudioService _audioService;
    private LevelService _levelService;
    private SettingsService _settingsService;
    private ServiceLocator _serviceLocator;

    private void Awake()
    {
        _serviceLocator = ServiceLocator.Current;
        Compose();
        Debug.Log("CompositeRoot Main Menu");
        YG2.saves.Init();
    }

    public override void Compose()
    {
        _wallet = _serviceLocator.Get<Wallet>();
        _levelService = _serviceLocator.Get<LevelService>();
        _audioService = _serviceLocator.Get<AudioService>();
        _settingsService = _serviceLocator.Get<SettingsService>();

        _wallet.Init();
        _walletView.Init();
        _audioService.Init(_uiAudioSource);

        _settingsService.Init();
    }
}
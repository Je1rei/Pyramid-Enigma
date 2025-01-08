using UnityEngine;
using YG;

namespace Source.Game.Scripts
{
    public class CompositeRootMainMenu : CompositeRoot
    {
        [SerializeField] private WalletView _walletView;
        [SerializeField] private AudioSource _uiAudioSource;

        private Wallet _wallet;
        private AudioService _audioService;
        private SettingsService _settingsService;
        private ServiceLocator _serviceLocator;

        private void Awake()
        {
            _serviceLocator = ServiceLocator.Current;
            Compose();
        }

        public override void Compose()
        {
            _wallet = _serviceLocator.Get<Wallet>();    
            _audioService = _serviceLocator.Get<AudioService>();
            _settingsService = _serviceLocator.Get<SettingsService>();

            _wallet.Init(YG2.saves.Score);
            _walletView.Init(_wallet);
            _audioService.Init(_uiAudioSource);

            _settingsService.Init();
        }
    }
}
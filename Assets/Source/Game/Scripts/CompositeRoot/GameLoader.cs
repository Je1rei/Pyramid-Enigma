using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

namespace Source.Game.Scripts
{
    public class GameLoader : MonoBehaviour
    {
        [SerializeField] private LevelService _levelService;
        [SerializeField] private AudioService _audioService;
        [SerializeField] private SwipeInputHandler _swipeInputHandler;
        [SerializeField] private InputPause _inputPauser;
        [SerializeField] private SceneLoaderService _loaderService;
        
        private ServiceLocator _serviceLocator;

        private void Awake() 
        {
            _serviceLocator = new ServiceLocator();
            RegisterServices();
            YG2.GameReadyAPI();

            DontDestroyOnLoad(gameObject);
            SceneManager.LoadScene(_loaderService.MainMenuScene);
        }

        private void RegisterServices()
        {
            _serviceLocator.Register(_levelService);
            _serviceLocator.Register(_audioService);
            _serviceLocator.Register(_swipeInputHandler);
            _serviceLocator.Register(_inputPauser);
            _serviceLocator.Register(_loaderService);
            _serviceLocator.Register(new SettingsService());
            _serviceLocator.Register(new RewardService());
            _serviceLocator.Register(new Wallet());
            _serviceLocator.Register(new BombWallet());
            _serviceLocator.Register(new TimerService());
            _serviceLocator.Register(new TutorialService());
            _serviceLocator.Register(new ExplodeService());
        }
    }
}
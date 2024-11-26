using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private RewardService _rewardService;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private TimerService _timeService;
    [SerializeField] private SettingsService _settingsService;
    [SerializeField] private LevelService _levelService;
    [SerializeField] private AudioService _audioService;
    [SerializeField] private SwipeInputHandler _swipeInputHandler;
    [SerializeField] private InputPause _inputPauser;

    private ServiceLocator _serviceLocator;

    private void Awake()
    {
        _serviceLocator = new ServiceLocator();
        RegisterServices();
        YG2.saves.Init();
        YG2.GameReadyAPI();

        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene(1);
    }

    private void RegisterServices()
    {
        _serviceLocator.Register(_settingsService);
        _serviceLocator.Register(_rewardService);
        _serviceLocator.Register(_wallet);
        _serviceLocator.Register(_timeService);
        _serviceLocator.Register(_levelService);
        _serviceLocator.Register(_audioService);
        _serviceLocator.Register(_swipeInputHandler);
        _serviceLocator.Register(_inputPauser);
    }
}
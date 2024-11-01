using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private AudioService _audioService;
    [SerializeField] private SwipeInputHandler _swipeInputHandler;
    [SerializeField] private InputPause _inputPauser;

    private ServiceLocator _serviceLocator;

    private void Awake()
    {
        _serviceLocator = new ServiceLocator();
        RegisterServices();

        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene(1);
    }

    private void RegisterServices()
    {
        _serviceLocator.Register(_audioService);
        _serviceLocator.Register(_swipeInputHandler);
        _serviceLocator.Register(_inputPauser);
    }
}
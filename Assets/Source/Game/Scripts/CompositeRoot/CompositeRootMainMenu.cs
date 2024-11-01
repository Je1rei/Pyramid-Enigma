public class CompositeRootMainMenu : CompositeRoot
{
    private AudioService _audioService;

    private ServiceLocator _serviceLocator;

    private void Awake()
    {
        _serviceLocator = ServiceLocator.Current;
        Compose();
    }

    public override void Compose()
    {
        _audioService = _serviceLocator.Get<AudioService>();
    }
}
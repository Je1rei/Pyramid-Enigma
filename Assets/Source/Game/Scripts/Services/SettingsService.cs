namespace Source.Game.Scripts
{
    public class SettingsService : IService
    {
        private AudioService _audioService;

        private readonly string[] _languages = new string[] { "ru", "en", "tr" };

        public void Init()
        {
            _audioService = ServiceLocator.Current.Get<AudioService>();
        }

        public void SetVolume(float value)
        {
            _audioService.SetVolume(value);
        }

        public string[] GetLanguages()
        {
            string[] languages = _languages;

            return languages;
        }
    }
}
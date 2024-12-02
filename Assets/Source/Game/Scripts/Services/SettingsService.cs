using UnityEngine;

public class SettingsService : MonoBehaviour, IService
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

namespace YG
{
    public partial class SavesYG
    {
        public float Volume;
    }
}

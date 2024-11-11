using UnityEngine;

public class AudioService : MonoBehaviour, IService
{
    private AudioSource _uiAudioSource;

    public void Init(AudioSource uiAudioSource) // прочитать про аудиосервисы и посмотреть у других челиксов
    {
        _uiAudioSource = uiAudioSource;
    }

    public void PlaySound()
    {
        _uiAudioSource.Play();
    }
}

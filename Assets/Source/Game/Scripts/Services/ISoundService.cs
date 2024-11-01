public interface ISoundService : IService
{
    public void PlaySound(string soundName);
    public void StopSound(string soundName);
    public void SetVolume(float volume);
}

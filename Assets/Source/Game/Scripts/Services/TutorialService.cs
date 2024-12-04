using UnityEngine;
using YG;

public class TutorialService : MonoBehaviour, IService
{
    private bool _isActive;

    public bool IsActive => _isActive;

    public void Init()
    {
        LevelService levelService = ServiceLocator.Current.Get<LevelService>();

        if (levelService.Index == 0 )
        {
            _isActive = true;
        }
    }

    public void Deactivate() => _isActive = false;
}
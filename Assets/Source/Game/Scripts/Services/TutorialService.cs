using UnityEngine;
using YG;

public class TutorialService : MonoBehaviour, IService
{
    private bool _isActive;

    public bool IsActive => _isActive;

    public void Init()
    {
        if (YG2.isFirstGameSession)
        {
            _isActive = true;
        }
    }

    public void Deactivate() => _isActive = false;
}
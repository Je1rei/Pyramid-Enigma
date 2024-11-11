using UnityEngine;

public class LevelService : MonoBehaviour, IService
{
    [SerializeField] private LevelData[] _levels;

    private LevelData _current;

    public LevelData Current => _current;

    public LevelData LoadLevel(int index)
    {
        if (index < 0 || index >= _levels.Length)
            return null;

        _current = _levels[index];

        return _current;
    }
}

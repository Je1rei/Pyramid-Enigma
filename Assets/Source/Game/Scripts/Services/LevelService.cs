using System.Collections.Generic;
using UnityEngine;
using YG;

public class LevelService : MonoBehaviour, IService
{
    [SerializeField] private LevelData[] _levels;

    private LevelData _current;
    private int _id;

    public int ID => _id;
    public LevelData Current => _current;

    public LevelData Load(int index)
    {
        if (index < 0 || index >= _levels.Length)
        {
            return null;
        }

        _current = _levels[index];
        _id = index;

        return _current;
    }

    public void Complete()
    {
        if (_id < _levels.GetLength(0) - 1)
        {
            _id++;

            if (_id <= _levels.GetLength(0))
            {
                YG2.saves.OpenedLevels.Add(_levels[_id].ID);
                YG2.SaveProgress();
            }
        }
    }

    public void CreateBackground(Transform parent)
    {
        Background background = Instantiate(_current.Background, parent);
        background.transform.SetAsFirstSibling();
    }
}

namespace YG
{
    public partial class SavesYG
    {
        public List<int> OpenedLevels = new List<int>();

        public void Init()
        {
            OpenedLevels.Add(1);
        }
    }
}
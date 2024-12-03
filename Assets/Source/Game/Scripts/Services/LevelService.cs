using System.Collections.Generic;
using UnityEngine;
using YG;

public class LevelService : MonoBehaviour, IService
{
    [SerializeField] private LevelData[] _levels;

    private LevelData _current;
    private int _index;

    public int Index => _index;
    public LevelData Current => _current;

    public void Init() { }

    public LevelData Load(int index)
    {
        if (index < 0 || index >= _levels.Length)
            return null;

        _current = _levels[index];
        _index = index;

        return _current;
    }    

    public void Complete()
    {
        if (_index < _levels.GetLength(0))
        {
            _index++;
            YG2.saves.OpenLevels[_index] = true;
            YG2.SaveProgress();
        }
    }

    public void CreateBackground(RectTransform parent)
    {
        Background background = Instantiate(_current.Background, parent);
        background.transform.SetAsFirstSibling();
    }
}

namespace YG
{
    public partial class SavesYG
    {
        public List<bool> OpenLevels = new List<bool>(new bool[12]);

        public void Init()
        {
            OpenLevels[0] = true;
        }
    }
}

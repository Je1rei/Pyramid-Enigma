using System.Collections.Generic;

namespace YG
{
    public partial class SavesYG
    {
        public float Volume = 0.25f;
        public int Bombs = 10;
        public int Score = 0;
        public List<int> OpenedLevels = new();

        public void Init()
        {
            OpenedLevels.Add(1);
        }
    }
}
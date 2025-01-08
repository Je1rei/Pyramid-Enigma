using UnityEngine;

namespace Source.Game.Scripts
{
    public static class Vector3Extensions
    {
        public static Vector3 CalculateCenter(this Vector3 _, int width, int height, int length, float cellSize)
        {
            float centerX = (width - 1) * 0.5f * cellSize;
            float centerY = (height - 1) * 0.5f * cellSize;
            float centerZ = (length - 1) * 0.5f * cellSize;

            return new Vector3(centerX, centerY, centerZ);
        }
    }
}

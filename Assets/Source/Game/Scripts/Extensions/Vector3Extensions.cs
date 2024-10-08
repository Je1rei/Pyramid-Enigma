using UnityEngine;

public static class Vector3Extensions
{
    public static float SqrDistance(this Vector3 start, Vector3 end)
    {
        return (end - start).sqrMagnitude;
    }
    //public static bool IsEnoughClose(this Vector3 start, Vector3 end, float distance)
    //{
    //    return start.SqrDistance(end) <= distance * distance;
    //}

    public static Vector3 CalculateCenter(this Vector3 vector, int width, int height, int length, float cellSize)
    {
        float centerX = (width - 1) * 0.5f * cellSize;
        float centerY = (height - 1) * 0.5f * cellSize;
        float centerZ = (length - 1) * 0.5f * cellSize;

        return new Vector3(centerX, centerY, centerZ);
    }
}

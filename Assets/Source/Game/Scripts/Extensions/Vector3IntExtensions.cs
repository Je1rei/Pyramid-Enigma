using UnityEngine;

public static class Vector3IntExtensions
{
    public static Vector3Int ToVector3Int(this DirectionType direction)
    {
        return direction switch
        {
            DirectionType.Left => new Vector3Int(1, 0, 0),
            DirectionType.Right => new Vector3Int(-1, 0, 0),
            DirectionType.Forward => new Vector3Int(0, 0, 1),
            DirectionType.Back => new Vector3Int(0, 0, -1),
            DirectionType.Up => new Vector3Int(0, 1, 0),
            DirectionType.Down => new Vector3Int(0, -1, 0),
            _ => Vector3Int.zero,
        };
    }
}
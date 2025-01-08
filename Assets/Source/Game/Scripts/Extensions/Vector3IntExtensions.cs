using UnityEngine;

namespace Source.Game.Scripts
{
    public static class Vector3IntExtensions
    {
        public static Vector3Int ToVector3Int(this DirectionType direction)
        {
            return direction switch
            {
                DirectionType.Left => Vector3Int.left,
                DirectionType.Right => Vector3Int.right,
                DirectionType.Forward => Vector3Int.forward,
                DirectionType.Back => Vector3Int.back,
                DirectionType.Up => Vector3Int.up,
                DirectionType.Down => Vector3Int.down,
                _ => Vector3Int.zero,
            };
        }
    }
}

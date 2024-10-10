using UnityEngine;

public static class BlockExtensions
{
    public static void UpdateDirectionAfterRotation(this Block block, Transform gridTransform)
    {
        Vector3Int localDirection = block.AllowedDirection.ToVector3Int();
        Vector3 worldDirection = gridTransform.TransformDirection(localDirection).normalized;

        DirectionType newDirection = worldDirection.ToDirectionType();
        block.SetAllowedDirection(newDirection);
    }
}

public static class DirectionTypeExtensions
{
    public static DirectionType ToOpposite(this DirectionType direction)
    {
        switch (direction)
        {
            case DirectionType.Forward: return DirectionType.Back;
            case DirectionType.Back: return DirectionType.Forward;
            case DirectionType.Up: return DirectionType.Down;
            case DirectionType.Down: return DirectionType.Up;
            case DirectionType.Left: return DirectionType.Right;
            case DirectionType.Right: return DirectionType.Left;
            default: return direction; 
        }
    }
}
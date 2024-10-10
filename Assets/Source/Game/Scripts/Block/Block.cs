using UnityEngine;

[RequireComponent(typeof(BlockMover), typeof(BlockShaker))]
public class Block : MonoBehaviour
{
    private const int _angleOffset = 90;
    [SerializeField] private DirectionType _allowedDirection;

    public Cell Cell { get; private set; }
    public DirectionType AllowedDirection => _allowedDirection;

    public void Init()
    {
        _allowedDirection = RandomizeDirection();
        Vector3Int direction = _allowedDirection.ToVector3Int();

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = targetRotation;
    }

    public void SetAllowedDirection(DirectionType newDirection)
    {
        _allowedDirection = newDirection; 
    }

    public void SetCurrentCell(Cell cell) => Cell = cell;

    private DirectionType RandomizeDirection()
    {
        DirectionType[] direction = (DirectionType[])System.Enum.GetValues(typeof(DirectionType));
        int randomIndex = Random.Range(1, direction.Length);

        return direction[randomIndex];
    }
}

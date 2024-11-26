using UnityEngine;

[RequireComponent(typeof(BlockMover), typeof(BlockShaker), typeof(TrailRenderer))]
public class Block : MonoBehaviour
{
    private DirectionType _allowedDirection;
    private TrailRenderer _trailRenderer;

    public TrailRenderer TrailRenderer => _trailRenderer;
    public Cell Cell { get; private set; }
    public Vector3Int ForwardDirection { get; private set; }

    public void Init()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
        _allowedDirection = RandomizeDirection();
        UpdateForwardDirection();
    }

    public void SetCurrentCell(Cell cell) => Cell = cell;

    public void SetAllowedDirection(DirectionType allowedDirection) => _allowedDirection = allowedDirection;

    public void UpdateForwardDirection()
    {
        ForwardDirection = _allowedDirection.ToVector3Int();
        Quaternion targetRotation = Quaternion.LookRotation(ForwardDirection, Vector3.up);
        transform.rotation = targetRotation;
    }

    public DirectionType RandomizeDirection()
    {
        DirectionType[] direction = (DirectionType[])System.Enum.GetValues(typeof(DirectionType));
        int randomIndex = Random.Range(1, direction.Length);

        return direction[randomIndex];
    }
}

using UnityEngine;

[RequireComponent(typeof(BlockMover))]
public class Block : MonoBehaviour
{
    private const int _angleOffset = 90;
    [SerializeField] private DirectionType _allowedDirection;

    public Cell CurrentCell { get; private set; }
    public DirectionType AllowedDirection => _allowedDirection;

    public void Init()
    {
        _allowedDirection = RandomizeDirection();
        Vector3Int direction = _allowedDirection.ToVector3Int();
        // Преобразуем вектор направления в угол для вращения
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = targetRotation;

        //_allowedDirection = RandomizeDirection();
        //transform.rotation = Quaternion.Euler(_allowedDirection.ToVector3Int() * _angleOffset);
    }

    public void SetCurrentCell(Cell cell) => CurrentCell = cell;

    private DirectionType RandomizeDirection()
    {
        DirectionType[] direction = (DirectionType[])System.Enum.GetValues(typeof(DirectionType));
        int randomIndex = Random.Range(1, direction.Length);

        return direction[randomIndex];
    }
}
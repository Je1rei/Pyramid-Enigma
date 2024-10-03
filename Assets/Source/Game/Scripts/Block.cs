using UnityEngine;

[RequireComponent (typeof(BlockMover))]
public class Block : MonoBehaviour
{
    [SerializeField] private DirectionType _allowedDirection;

    public Cell CurrentCell { get; private set; }
    public DirectionType AllowedDirection => _allowedDirection;

    public void SetCurrentCell(Cell cell) => CurrentCell = cell;
}

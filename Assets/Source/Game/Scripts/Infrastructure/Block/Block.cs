using UnityEngine;

[RequireComponent(typeof(BlockMover), typeof(BlockShaker), typeof(TrailRenderer))]
[RequireComponent(typeof(MeshRenderer), typeof(BlockExploder), typeof(AudioSource))]
public class Block : MonoBehaviour
{
    private DirectionType _allowedDirection;
    private TrailRenderer _trailRenderer;
    private MeshRenderer _meshRenderer;
    private Color _initColor;
    private AudioSource _audioSource;
    private BlockMover _mover;

    public MeshRenderer Renderer => _meshRenderer;
    public TrailRenderer TrailRenderer => _trailRenderer;
    public Cell Cell { get; private set; }
    public Vector3Int ForwardDirection { get; private set; }

    public void Init()
    {
        _mover = GetComponent<BlockMover>();
        _audioSource = GetComponent<AudioSource>();
        _trailRenderer = GetComponent<TrailRenderer>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _allowedDirection = RandomizeDirection();
        _initColor = _meshRenderer.material.color;

        SetAllowedDirection(_allowedDirection);
        UpdateForwardDirection();

        _mover.Init();
    }

    public void PlaySound()
    {
        _audioSource.Play();
    }

    public void SetCurrentCell(Cell cell)
    {
        Cell = cell;
    }

    public void SetAllowedDirection(DirectionType allowedDirection) => _allowedDirection = allowedDirection;

    public DirectionType GetAllowedDirection() => _allowedDirection;

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

    public void RandomizeColor(Color[] palette)
    {
        Color randomColor = palette[Random.Range(0, palette.Length)];

        if (TryGetComponent(out Renderer renderer))
            renderer.material.color = randomColor;
    }

    public void ResetColor() => _meshRenderer.material.color = _initColor;
}

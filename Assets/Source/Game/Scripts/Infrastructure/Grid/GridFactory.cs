using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CellFactory))]
public class GridFactory : MonoBehaviour
{
    [SerializeField] private float _rotateAngle = 360;
    [SerializeField] private float _spawnDuration = 1f;
    [SerializeField] private float _delay = 0.1f;
    [SerializeField] private float _multiplierStartPosition = 30;

    private InputPause _inputPauser;
    private CellFactory _cellFactory;
    private Sequence _sequence;

    public void Init()
    {
        _inputPauser = ServiceLocator.Current.Get<InputPause>();
        _cellFactory = GetComponent<CellFactory>();
    }

    public Cell[,,] Create(GridData data, Grid gridParent, Vector3 center)
    {
        _sequence = DOTween.Sequence();
        _sequence.AppendCallback(() => { _inputPauser.DeactivateInput(); });

        Cell[,,] grid = new Cell[data.Width, data.Length, data.Height];

        for (int x = 0; x < data.Width; x++)
        {
            for (int z = 0; z < data.Length; z++)
            {
                for (int y = 0; y < data.Height; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, z);
                    Cell cell = _cellFactory.Create(data.CellPrefab, position, gridParent);

                    Vector3 randomStartPosition =
                        center + Random.onUnitSphere * data.CellSize * _multiplierStartPosition;
                    cell.transform.position = randomStartPosition;

                    Block block = Instantiate(data.BlockPrefab, cell.transform);
                    block.SetCurrentCell(cell);
                    block.RandomizeColor(data.PaletteData.Palette);
                    block.Init();

                    cell.SetOccupy(block);
                    grid[x, y, z] = cell;

                    Vector3 targetPosition = new Vector3(x * data.CellSize, y * data.CellSize, z * data.CellSize);
                    float delay = _delay * (x + y + z);

                    _sequence.Insert(delay,
                        cell.transform.DOMove(targetPosition, _spawnDuration).SetEase(Ease.OutBack));
                    _sequence.Insert(delay,
                        cell.transform.DORotate(Vector3.one * _rotateAngle, _spawnDuration, RotateMode.FastBeyond360));

                    block.TrailRenderer.enabled = true;
                }
            }
        }

        _sequence.OnComplete(() =>
        {
            _inputPauser.ActivateInput();
        });

        return grid;
    }
}
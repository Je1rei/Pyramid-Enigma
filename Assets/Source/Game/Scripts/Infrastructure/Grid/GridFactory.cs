using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CellFactory))]
public class GridFactory : MonoBehaviour
{
    [SerializeField] private float _rotateAngle = 180;
    [SerializeField] private float _spawnDuration = 0.2f;
    [SerializeField] private float _delay = 0.1f;

    private float _multiplierStartPosition = 2f;
    private float _minPositionMultiplier = -0.2f;
    private float _maxPositionMultiplier = 0.2f;
    
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

                    float reducedRadius = data.CellSize * _multiplierStartPosition;
                    Vector3 randomStartPosition = center + Random.insideUnitSphere * reducedRadius;
                    cell.transform.position = randomStartPosition;

                    Block block = Instantiate(data.BlockPrefab, cell.transform);
                    block.SetCurrentCell(cell);
                    block.RandomizeColor(data.PaletteData.Palette);
                    block.Init();

                    block.transform.localScale = Vector3.zero;

                    cell.SetOccupy(block);
                    grid[x, y, z] = cell;

                    Vector3 intermediatePosition = center + new Vector3(
                        (x + Random.Range(_minPositionMultiplier, _maxPositionMultiplier)) * data.CellSize,
                        (y + Random.Range(_minPositionMultiplier, _maxPositionMultiplier)) * data.CellSize,
                        (z + Random.Range(_minPositionMultiplier, _maxPositionMultiplier)) * data.CellSize
                    );

                    float delay = _delay * (x + y + z);

                    _sequence.Insert(delay,
                        cell.transform.DOMove(intermediatePosition, _spawnDuration).SetEase(Ease.OutQuad));
                    _sequence.Insert(delay,
                        cell.transform.DORotate(Vector3.one * (_rotateAngle), _spawnDuration,
                            RotateMode.FastBeyond360));

                    _sequence.Insert(delay,
                        block.transform.DOScale(Vector3.one, _spawnDuration)
                            .SetEase(Ease.OutBack));
                    _sequence.Insert(delay + _spawnDuration,
                        cell.transform.DOMove(new Vector3(x * data.CellSize, y * data.CellSize,
                            z * data.CellSize), _spawnDuration).SetEase(Ease.OutBack));

                    block.TrailRenderer.enabled = true;
                }
            }
        }

        _sequence.OnComplete(() => { _inputPauser.ActivateInput(); });

        return grid;
    }
}
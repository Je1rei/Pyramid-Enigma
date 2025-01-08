using DG.Tweening;
using UnityEngine;

namespace Source.Game.Scripts
{
    [RequireComponent(typeof(CellFactory))]
    public class GridFactory : MonoBehaviour
    {
        [SerializeField] private float _rotateAngle = 180;
        [SerializeField] private float _spawnDuration = 0.5f;
        [SerializeField] private float _delay = 0.1f;

        private readonly float _multiplierStartPosition = 5f;
        private readonly float _minPositionMultiplier = -0.1f;
        private readonly float _maxPositionMultiplier = 0.1f;

        private DirectionType _randomDirection;
        private InputPause _inputPauser;
        private CellFactory _cellFactory;
        private Sequence _sequence;

        public void Init()
        {
            _inputPauser = ServiceLocator.Current.Get<InputPause>();
            _cellFactory = GetComponent<CellFactory>();
            _randomDirection = RandomizeDirection();
        }

        public Cell[,,] Create(GridData data, Grid gridParent, GridRotator rotator, Vector3 center)
        {
            _sequence = DOTween.Sequence();
            _sequence.AppendCallback(() => { _inputPauser.DeactivateInput(); });

            Cell[,,] predefinedGrid = ParseGridFromPrefab(data, gridParent, center);

            _sequence.OnComplete(() =>
            {
                rotator.Init(gridParent.Center, _randomDirection);
                _inputPauser.ActivateInput();
            });

            return predefinedGrid;
        }

        private Cell[,,] ParseGridFromPrefab(GridData data, Grid gridParent, Vector3 center)
        {
            Cell[,,] grid = new Cell[data.Width, data.Height, data.Length];
            var cells = data.GridPrefab.GetComponentsInChildren<Cell>();

            if (cells.Length != data.Width * data.Height * data.Length)
            {
                return null;
            }

            int index = 0;

            for (int x = 0; x < data.Width; x++)
            {
                for (int y = 0; y < data.Height; y++)
                {
                    for (int z = 0; z < data.Length; z++)
                    {
                        Cell cellPrefab = cells[index];
                        index++;

                        Vector3Int position = new Vector3Int(x, y, z);
                        Cell cell = _cellFactory.Create(cellPrefab, position, gridParent);
                        Block block = cell.Occupied;

                        float reducedRadius = data.CellSize * _multiplierStartPosition;
                        Vector3 randomStartPosition = center + Random.insideUnitSphere * reducedRadius;
                        cell.transform.position = randomStartPosition;

                        block.RandomizeColor(data.PaletteData.Palette);
                        block.Init();
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
                            cell.transform.DORotate(Vector3.one * _rotateAngle, _spawnDuration,
                                RotateMode.FastBeyond360));
                        _sequence.Insert(delay,
                            block.transform.DOScale(Vector3.one, _spawnDuration).SetEase(Ease.OutBack));
                        _sequence.Insert(delay + _spawnDuration,
                            cell.transform.DOMove(new Vector3(x * data.CellSize, y * data.CellSize,
                                z * data.CellSize), _spawnDuration).SetEase(Ease.OutBack));

                        block.TrailRenderer.enabled = true;
                    }
                }
            }

            return grid;
        }

        private DirectionType RandomizeDirection()
        {
            DirectionType[] direction = { DirectionType.Left, DirectionType.Right, DirectionType.Up, DirectionType.Down };

            int randomIndex = Random.Range(0, direction.Length);

            return direction[randomIndex];
        }
    }
}
using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(InputPause))]
public class SwipeInputHandler : MonoBehaviour, IService
{
    [SerializeField] private float _minSwipeDistance = 50f;
    [SerializeField] private float _directionTolerance = 0.2f;

    private ExplodeService _explosionService;
    private TutorialService _tutorialService;
    private GridRotator _rotator;
    private InputPause _inputPause;

    private Vector3 _mousePositionStart;
    private Vector3 _mousePositionEnd;
    private bool _isSwiping;

    private int _movingBlocksCount;

    public event Action Clicked;

    public void Init(Grid grid)
    {
        _explosionService = ServiceLocator.Current.Get<ExplodeService>();
        _inputPause = GetComponent<InputPause>();
        _tutorialService = ServiceLocator.Current.Get<TutorialService>();
        _rotator = grid.Rotator;
        _movingBlocksCount = 0;
        _isSwiping = false;
    }

    public void Tick()
    {
        if (_rotator != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mousePositionStart = Input.mousePosition;
                _isSwiping = false;
            }

            if (Input.GetMouseButton(0))
            {
                _mousePositionEnd = Input.mousePosition;

                if ((_mousePositionEnd - _mousePositionStart).magnitude > _minSwipeDistance)
                {
                    _isSwiping = true;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _mousePositionEnd = Input.mousePosition;

                if (_isSwiping && _inputPause.CanInput && _movingBlocksCount == 0)
                {
                    SwipeDetect();
                }
                else if (_isSwiping == false)
                {
                    HandleClick();
                }
            }
        }
    }

    private void SwipeDetect()
    {
        Vector3 delta = _mousePositionEnd - _mousePositionStart;
        DirectionType swipeDirection = CalculateDirection(delta);

        _rotator.Rotate(swipeDirection);

        _inputPause.StartCooldown(_rotator.Duration);
    }

    private void HandleClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.TryGetComponent(out Block block) && _tutorialService.IsActive == false)
        {
            if (block.TryGetComponent(out BlockMover mover))
            {
                if (mover.IsMoving)
                    return;

                _movingBlocksCount++;
                mover.Moved += OnMoveCompleted;

                if (_explosionService.IsActive == false)
                {
                    mover.SetupMove();

                    if (mover.IsMoving == false)
                    {
                        _movingBlocksCount--;
                        mover.Moved -= OnMoveCompleted;
                    }
                }
                else
                {
                    mover.Explode();
                }
            }
        }

        Clicked?.Invoke();
    }

    private DirectionType CalculateDirection(Vector3 delta)
    {
        if (Mathf.Abs(delta.y) > Mathf.Abs(delta.x) + _directionTolerance)
            return delta.y > 0 ? DirectionType.Left : DirectionType.Right;
        else if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y) + _directionTolerance)
            return delta.x > 0 ? DirectionType.Down : DirectionType.Up;
        else
            return DirectionType.None;
    }

    private void OnMoveCompleted(BlockMover mover)
    {
        _movingBlocksCount--;
        mover.Moved -= OnMoveCompleted;

        if(_movingBlocksCount < 0)
        {
            _movingBlocksCount = 0;
        }
    }
}

using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(InputPause))]
public class SwipeInputHandler : MonoBehaviour
{
    [SerializeField] private float _minDistance = 1f;
    [SerializeField] private float _directionTolerance = 0.2f;
    [SerializeField] private GridRotator _rotator;

    private InputPause _inputPause;
    private Vector3 _mousePositionStart;
    private Vector3 _mousePositionEnd;

    private bool _isSwiping;

    private void Awake()
    {
        _inputPause = GetComponent<InputPause>();
    }

    private void Update()
    {
        if (_inputPause.CanInput())
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mousePositionStart = Input.mousePosition;
                _isSwiping = false;
            }

            if (Input.GetMouseButton(0))
            {
                _mousePositionEnd = Input.mousePosition;
                float distance = _mousePositionStart.SqrDistance(_mousePositionEnd); // ДОДЕЛАТЬ ПЕРЕДЕЛАТЬ

                if (distance > _minDistance)
                {
                    _isSwiping = true;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _mousePositionEnd = Input.mousePosition;

                if (_isSwiping)
                {
                    SwipeDetect();
                }
                else
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
    }

    private void HandleClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.TryGetComponent(out BlockMover mover))
            {
                mover.TryGetComponent(out Block block);
                mover.SetupMove();
            }
        }
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
}

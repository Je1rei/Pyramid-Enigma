using UnityEngine;
using UnityEngine.UIElements;

public class SwipeInputHandler : MonoBehaviour
{
    [SerializeField] private float _minDistance = 1f;
    [SerializeField] private float _directionTolerance = 0.2f;
    [SerializeField] private GridRotator _rotator;

    private Vector3 _mousePositionStart;
    private Vector3 _mousePositionEnd;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mousePositionStart = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _mousePositionEnd = Input.mousePosition;

            if (_mousePositionStart.SqrDistance(_mousePositionEnd) > _minDistance)
            {
                SwipeDetect();
            }
            else
            {
                HandleClick();
            }
        }
    }

    private void SwipeDetect()
    {
        Vector3 delta = _mousePositionEnd - _mousePositionStart;

        DirectionType swipeDirection = CalculateDirection(delta);

        _rotator.SetupRotate(swipeDirection);
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
                mover.SetupMove(block.AllowedDirection);
            }
        }
    }

    private DirectionType CalculateDirection(Vector3 delta)
    {
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y) + _directionTolerance && Mathf.Abs(delta.x) > Mathf.Abs(delta.z) + _directionTolerance)
            return delta.x > 0 ? DirectionType.Right : DirectionType.Left;
        else if (Mathf.Abs(delta.y) > Mathf.Abs(delta.x) + _directionTolerance && Mathf.Abs(delta.y) > Mathf.Abs(delta.z) + _directionTolerance)
            return delta.y > 0 ? DirectionType.Up : DirectionType.Down;

        return DirectionType.None;
    }
}

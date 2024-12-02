using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _angleX = 33f;
    [SerializeField] private float _angleY = -140f;
    [SerializeField] private Grid _grid;

    private float _multiplier = 4f;
    private Vector3 _targetPosition;
    private Vector3 _gridCenter;

    public void Init()
    {
        SetupCameraPosition();
    }

    public float GetDistance()
    {
        return Vector3.Distance(_cameraTransform.position, _gridCenter);
    }

    private void SetupCameraPosition()
    {
         _gridCenter = _grid.Center;

        float gridSize = Mathf.Max(_grid.Data.Width, _grid.Data.Height, _grid.Data.Length) * _grid.Data.CellSize;
        float distance = gridSize * _multiplier;

        _targetPosition = _gridCenter - Quaternion.Euler(_angleX, _angleY, 0) * Vector3.forward * distance;
        _cameraTransform.position = _targetPosition;
        _cameraTransform.rotation = Quaternion.Euler(_angleX, _angleY, 0);
    }
}

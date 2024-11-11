using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _angleX = 33f;
    [SerializeField] private float _angleY = -140f;
    [SerializeField] private Grid _grid;

    private float _multiplier = 4f;
    private Vector3 _targetPosition;

    public void Init()
    {
        SetupCameraPosition();
    }

    private void SetupCameraPosition()
    {
        Vector3 gridCenter = _grid.Center;

        float gridSize = Mathf.Max(_grid.Data.Width, _grid.Data.Height, _grid.Data.Length) * _grid.Data.CellSize;
        float distance = gridSize * _multiplier;

        _targetPosition = gridCenter - Quaternion.Euler(_angleX, _angleY, 0) * Vector3.forward * distance;
        _cameraTransform.position = _targetPosition;
        _cameraTransform.rotation = Quaternion.Euler(_angleX, _angleY, 0);
    }
}

using System.Collections;
using UnityEngine;
using DG.Tweening;

public class GridRotator : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private int _endAngleRotation = 90;

    private Grid _grid;

    public bool IsRotating { get; private set; } = false;

    private void Awake()
    {
        _grid = GetComponent<Grid>();
        transform.position += _grid.Center;
    }

    public void Rotate(DirectionType direction)
    {
        IsRotating = true;
        Vector3 rotateDirection = direction.ToVector3Int();

        float fixedZRotation = transform.eulerAngles.z;

        Vector3 rotationAxis = rotateDirection.normalized;

        float currentAngle = 0f;
        float targetAngle = _endAngleRotation;

        DOTween.To(() => currentAngle, x =>
        {
            float deltaAngle = x - currentAngle;
            currentAngle = x;

            transform.RotateAround(_grid.Center, rotationAxis, deltaAngle);

        }, targetAngle, _duration)
        .SetEase(Ease.InOutQuad)
        .OnComplete(() => IsRotating = false);
    }
}
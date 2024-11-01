using UnityEngine;
using DG.Tweening;

public class GridRotator : MonoBehaviour
{
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private int _endAngleRotation = 90;

    private Vector3 _center;

    public bool IsRotating { get; private set; } = false;
    public float Duration => _duration;

    public void Init(Vector3 center)
    {
        _center = transform.position + center;
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

            transform.RotateAround(_center, rotationAxis, deltaAngle);

        }, targetAngle, _duration)
        .SetEase(Ease.InOutQuad)
        .OnComplete(() => IsRotating = false);
    }
}
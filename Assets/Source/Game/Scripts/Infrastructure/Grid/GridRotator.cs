using DG.Tweening;
using UnityEngine;

namespace Source.Game.Scripts
{
    public class GridRotator : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.25f;
        [SerializeField] private int _endAngleRotation = 90;

        private Vector3 _center;

        public bool IsRotating { get; private set; } = false;
        public float Duration => _duration;

        public void Init(Vector3 center, DirectionType direction)
        {
            _center = transform.position + center;
            Rotate(direction);
        }

        public void Rotate(DirectionType direction)
        {
            IsRotating = true;
            Vector3 rotateDirection = direction.ToVector3Int();
            Vector3 rotationAxis = rotateDirection.normalized;

            float currentAngle = 0f;
            float targetAngle = _endAngleRotation;

            DOVirtual.Float(currentAngle, targetAngle, _duration, x =>
                {
                    if (this != null && transform != null)
                    {
                        float deltaAngle = x - currentAngle;
                        currentAngle = x;
                        transform.RotateAround(_center, rotationAxis, deltaAngle);
                    }
                })
                .SetEase(Ease.InOutQuad)
                .OnComplete(() => IsRotating = false);
        }
    }
}
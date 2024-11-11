using DG.Tweening;
using UnityEngine;

public class TreasureMover : MonoBehaviour
{
    [SerializeField] private float _moveDuration = 1f;
    [SerializeField] private float _rotateDuration = 3f;
    [SerializeField] private float _moveOffsetY = 1f;
    [SerializeField] private Vector3 _targetScale = new Vector3(3f, 3f, 3f);

    private Vector3 _rotateAngleY = new Vector3(0, 360, 0);

    public void Move()
    {
        transform.DOMoveY(transform.position.y + _moveOffsetY, _moveDuration);
        transform.DOScale(_targetScale, _moveDuration).SetEase(Ease.OutQuad);

        transform.DORotate(_rotateAngleY, _rotateDuration, RotateMode.FastBeyond360)
                 .SetLoops(-1, LoopType.Incremental)
                 .SetEase(Ease.Linear);
    }
}
using DG.Tweening;
using UnityEngine;

public class BlockShaker : MonoBehaviour
{
    [SerializeField] private float _strength = 0.1f;
    [SerializeField] private float _duration = 0.25f;
    [SerializeField] private int _countVibration = 10;
    [SerializeField] private float _randomAngle = 0f;

    private Vector3 _initialPosition;
    private Tween _tween;

    public void Shake()
    {
        if (_tween != null && _tween.IsActive())
        {
            _tween.Kill();
            transform.position = _initialPosition;
        }

        _initialPosition = transform.position;

        _tween = transform.DOShakePosition(_duration, _strength, _countVibration, _randomAngle, false, true)
            .OnComplete(() => transform.position = _initialPosition); 
    }
}

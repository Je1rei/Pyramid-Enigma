using DG.Tweening;
using UnityEngine;

public class BlockShaker : MonoBehaviour
{
    [SerializeField] private float _strength = 0.1f;
    [SerializeField] private float _duration = 0.25f;

    private Color _shakeColor = Color.red;
    private Vector3 _initialPosition;
    private Tween _tween;

    private Block _block;

    public void Init()
    {
        _block = GetComponent<Block>();
    }

    public void Shake()
    {
        if (_tween != null && _tween.IsActive())
        {
            transform.position = _initialPosition;

            return;
        }

        _initialPosition = transform.position;
        Vector3 shakeDirection = (Vector3)_block.ForwardDirection * _strength;
        _block.Renderer.material.color = _shakeColor;

        _tween = transform.DOShakePosition(_duration, shakeDirection)
            .OnComplete(() =>
            {
                _block.ResetColor();
                transform.position = _initialPosition;
            });
    }
}

using DG.Tweening;
using UnityEngine;

public class BlockExploder : MonoBehaviour
{
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private float _minScale = 0f;

    private bool _isExplodes;
    private BombWallet _bombWallet;
    private MeshRenderer _renderer;

    public void Init(MeshRenderer renderer)
    {
        _renderer = renderer;
        _bombWallet = ServiceLocator.Current.Get<BombWallet>();
        _isExplodes = false;
    }

    public bool TryExplode()
    {
        if (_bombWallet.Bombs > 0 && _isExplodes == false)
        {
            _isExplodes = true;
            _renderer.material.DOFade(0, _duration);

            transform.DOScale(_minScale, _duration)
                .OnKill(() => Destroy(gameObject));

            _bombWallet.DecreaseScore();

            return true;
        }

        return false;
    }
}
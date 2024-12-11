using DG.Tweening;
using UnityEngine;

public class BlockExploder : MonoBehaviour
{
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private float _minScale = 0f;

    public bool _isExplodes;

    private void OnEnable()
    {
        _isExplodes = false;
    }

    public bool TryExplode()
    {
        BombWallet bombWallet = ServiceLocator.Current.Get<BombWallet>();

        if (bombWallet.Bombs > 0 && _isExplodes == false)
        {
            _isExplodes = true;
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

            meshRenderer.material.DOFade(0, _duration);

            transform.DOScale(_minScale, _duration)
                .OnKill(() => Destroy(gameObject));

            bombWallet.DecreaseScore();

            return true;
        }

        return false;
    }
}
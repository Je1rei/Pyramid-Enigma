﻿using DG.Tweening;
using UnityEngine;

namespace Source.Game.Scripts
{
    public class BlockExploder : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _minScale = 0f;
    
        private Color _explosionColor = Color.red;
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
            if (_bombWallet.Value > 0 && _isExplodes == false)
            {
                _isExplodes = true;

                _renderer.material.DOColor(_explosionColor, _duration);
            
                transform.DOScale(_minScale , _duration )
                    .OnKill(() => Destroy(gameObject));

                _bombWallet.Decrease();

                return true;
            }

            return false;
        }
    }
}
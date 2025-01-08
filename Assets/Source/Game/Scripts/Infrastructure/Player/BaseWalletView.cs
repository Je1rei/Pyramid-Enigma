using TMPro;
using UnityEngine;

namespace Source.Game.Scripts
{
    public abstract class BaseWalletView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        
        private BaseWallet _wallet;
        
        private void OnDisable()
        {
            _wallet.Changed -= Change;
        }
        
        public void Init(BaseWallet wallet)
        {
            _wallet = wallet;

            Change(_wallet.Value);
            _wallet.Changed += Change;
        }
        
        private void Change(int value)
        {
            _text.text = value.ToString();
        }   
    }
}
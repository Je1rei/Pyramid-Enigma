using TMPro;
using UnityEngine;
using YG;

public class WalletView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private Wallet _wallet;

    private void OnDisable()
    {
        _wallet.Changed -= Change;
    }

    public void Init()
    {
        _wallet = ServiceLocator.Current.Get<Wallet>();

        Change(_wallet.Score);
        _wallet.Changed += Change;
    }

    private void Change(int value)
    {
        _text.text = value.ToString();
    }    
}
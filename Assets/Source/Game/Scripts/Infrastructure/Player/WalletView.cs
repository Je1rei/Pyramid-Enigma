using TMPro;
using UnityEngine;

public class WalletView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private Wallet _wallet;

    private void OnDisable()
    {
        _wallet.ScoreChanged -= Change;
    }

    public void Init()
    {
        _wallet = ServiceLocator.Current.Get<Wallet>();

        Change(_wallet.Score);
        _wallet.ScoreChanged += Change;
    }

    private void Change(int value)
    {
        _text.text = value.ToString();
    }    
}
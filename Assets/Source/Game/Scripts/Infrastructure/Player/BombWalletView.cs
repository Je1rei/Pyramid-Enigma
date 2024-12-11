using TMPro;
using UnityEngine;

public class BombWalletView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private BombWallet _wallet;

    private void OnDisable()
    {
        _wallet.CountChanged -= Change;
    }

    public void Init()
    {
        _wallet = ServiceLocator.Current.Get<BombWallet>();

        Change(_wallet.Bombs);
        _wallet.CountChanged += Change;
    }

    private void Change(int value)
    {
        _text.text = value.ToString();
    }
}

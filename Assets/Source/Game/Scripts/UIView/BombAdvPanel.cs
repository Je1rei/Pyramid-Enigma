using UnityEngine;
using UnityEngine.UI;
using YG;

public class BombAdvPanel : UIPanel
{
    [SerializeField] private int _rewardValue = 10;
    [SerializeField] private Button _agreeButton;
    [SerializeField] private Button _disagreeButton;
    [SerializeField]private BombPanel _bombPanel;
    
    private InputPause _inputPauser;

    private string _rewardID = "1";

    private void OnEnable()
    {
        _inputPauser = ServiceLocator.Current.Get<InputPause>();
        SetAudioService();
        AddButtonListener(_agreeButton, AgreeADV);
        AddButtonListener(_disagreeButton, DisagreeADV);
    }

    private void OnDisable()
    {
        _agreeButton.onClick.RemoveAllListeners();
        _disagreeButton.onClick.RemoveAllListeners();
    }

    private void AgreeADV()
    {
        YG2.RewardedAdvShow(_rewardID, () =>
        {
            ServiceLocator.Current.Get<BombWallet>().IncreaseScore(_rewardValue);
        });

        _inputPauser.ActivateInputCooldown();
        Hide();
        _bombPanel.DeactivateExplosionService();
    }    
    
    private void DisagreeADV()
    {
        _inputPauser.ActivateInputCooldown();
        Hide();
        _bombPanel.DeactivateExplosionService();
    }
}
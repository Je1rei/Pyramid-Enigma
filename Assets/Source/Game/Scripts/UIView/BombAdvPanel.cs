using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Source.Game.Scripts
{
    public class BombAdvPanel : UIPanel
    {
        private const string RewardID = "1";
    
        [SerializeField] private int _rewardValue = 3;
        [SerializeField] private Button _agreeButton;
        [SerializeField] private Button _disagreeButton;
        [SerializeField]private BombPanel _bombPanel;

        private InputPause _inputPauser;

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
            YG2.RewardedAdvShow(RewardID, () =>
            {
                ServiceLocator.Current.Get<BombWallet>().Increase(_rewardValue);
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
}
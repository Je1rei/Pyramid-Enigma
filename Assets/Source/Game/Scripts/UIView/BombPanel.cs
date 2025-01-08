using UnityEngine;
using UnityEngine.UI;

namespace Source.Game.Scripts
{
    public class BombPanel : UIPanel
    {
        [SerializeField] private BombAdvPanel _bombAdvPanel;
        [SerializeField] private Button _toggleButton;
        [SerializeField] private Image _activeBombesImage;

        private ExplodeService _explosionService;

        private void OnDisable()
        {
            _toggleButton.onClick.RemoveAllListeners();
            _explosionService.BombIsEmpty -= BombIsEmpty;
        }

        private void OnEnable()
        {
            _explosionService = ServiceLocator.Current.Get<ExplodeService>();
            SetAudioService();
            AddButtonListener(_toggleButton, ToggleActiveBombsImage);
        
            _explosionService.BombIsEmpty += BombIsEmpty;
            DeactivateExplosionService();
        }

        public void DeactivateExplosionService()
        {
            _explosionService.Deactivate();
            _activeBombesImage.gameObject.SetActive(false);
        }
    
        private void ToggleActiveBombsImage()
        {
            if(_explosionService.IsActive == true)
            {
                DeactivateExplosionService();
            }
            else
            {
                _explosionService.Activate();
                _activeBombesImage.gameObject.SetActive(true);
            }
        }

        private void BombIsEmpty()
        {
            _explosionService.Deactivate();
            _activeBombesImage.gameObject.SetActive(false);
            _bombAdvPanel.Show();
        }
    }
}

namespace Source.Game.Scripts
{
    public class TutorialService :IService
    {
        private bool _isActive;

        public bool IsActive => _isActive;

        public void Init()
        {
            LevelService levelService = ServiceLocator.Current.Get<LevelService>();

            if (levelService.ID == 0 )
            {
                _isActive = true;
            }
        }

        public void Deactivate() => _isActive = false;
    }
}
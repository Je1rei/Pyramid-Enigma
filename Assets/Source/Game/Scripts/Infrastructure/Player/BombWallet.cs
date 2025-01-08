using YG;

namespace Source.Game.Scripts
{
    public class BombWallet : BaseWallet
    {
        public override void Increase(int amount = 1)
        {
            base.Increase(amount);
            
            YG2.saves.Bombs = Value;
            YG2.SaveProgress();
        }

        public override void Decrease()
        {
            base.Decrease();
            
            YG2.saves.Bombs = Value;
            YG2.SaveProgress();
        }
    }
}
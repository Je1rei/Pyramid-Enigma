using YG;
namespace Source.Game.Scripts
{
    public class Wallet : BaseWallet
    {
        public override void Increase(int amount)
        {
            base.Increase(amount);
            YG2.saves.Score = Value;

            YG2.SetLeaderboard(nameLB: "Score", score: YG2.saves.Score);
            YG2.SaveProgress();
        }
    }
}
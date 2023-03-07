using Challenges.Listeners;

namespace Challenges.Challenges
{
    public class HoarderChallenge : Challenge, IEndOfBattleListener
    {
        private const int BASE_GOLD_REQUIRED = 15;

        protected override string GetDescription()
        {
            return "End a battle with " + RequiredProgress + " gold";
        }

        protected override int GetRequiredProgress(int level)
        {
            return (level + 1) * BASE_GOLD_REQUIRED;
        }

        public void EndOfBattle()
        {
            Progress = RunProgress.PlayerProgress.Gold;
            if (RunProgress.CurrentEvent == DisturbanceType.GoldRush)
                Progress += DisturbanceManager.GetDisturbance(DisturbanceType.GoldRush).amount;
        }
    }
}
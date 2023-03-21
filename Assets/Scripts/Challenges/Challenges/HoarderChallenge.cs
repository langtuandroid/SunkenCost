using Disturbances;
using EventListeners;

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
            Progress = RunProgress.PlayerStats.Gold;
        }
    }
}
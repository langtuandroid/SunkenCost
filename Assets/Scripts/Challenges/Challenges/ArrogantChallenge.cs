using Challenges.Listeners;

namespace Challenges.Challenges
{
    public class ArrogantChallenge : Challenge, IStartOfBattleListener
    {
        private const int BASE_ROUNDS_REQUIRED = 1;

        protected override string GetDescription()
        {
            return "Start " + RequiredProgress + " consecutive battles with less planks than you're allowed";
        }

        protected override int GetRequiredProgress(int level)
        {
            return level + BASE_ROUNDS_REQUIRED;
        }

        public void StartOfBattle()
        {
            if (StickManager.current.stickCount >= RunProgress.PlayerProgress.MaxPlanks)
            {
                Progress = 0;
                return;
            }

            Progress++;
        }
    }
}
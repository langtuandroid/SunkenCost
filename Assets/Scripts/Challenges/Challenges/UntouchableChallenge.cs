using EventListeners;

namespace Challenges.Challenges
{
    public class UntouchableChallenge : Challenge, IPlayerLostLifeListener, IEndOfBattleListener
    {
        private const int BASE_RUNS_REQUIRED = 2;

        protected override string GetDescription()
        {
            return "Finish " + RequiredProgress + " consecutive battles without losing a life";
        }

        protected override int GetRequiredProgress(int level)
        {
            return level + BASE_RUNS_REQUIRED;
        }
        
        public void PlayerLostLife()
        {
            Progress = -1;
        }

        public void EndOfBattle()
        {
            Progress++;
        }
    }
}
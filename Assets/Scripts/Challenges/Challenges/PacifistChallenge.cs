using EventListeners;

namespace Challenges.Challenges
{
    public class PacifistChallenge : Challenge, IKillListener, IEndOfBattleListener
    {
        private const int BASE_RUNS_REQUIRED = 2;
        
        protected override string GetDescription()
        {
            return "Kill no enemies in " + RequiredProgress + " consecutive battles";
        }

        protected override int GetRequiredProgress(int level)
        {
            return level + BASE_RUNS_REQUIRED;
        }

        public void EnemyKilled()
        {
            Progress = -1;
        }

        public void EndOfBattle()
        {
            Progress++;
        }
    }
}
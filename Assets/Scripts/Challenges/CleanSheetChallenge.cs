using Challenges.Listeners;
using UnityEngine;

namespace Challenges
{
    public class CleanSheetChallenge : Challenge, IKillListener, IEndOfBattleListener
    {
        private int _numberOfCleanSheetsRequired = 3;
        private int _numberOfCleanSheetsAchieved = 0;
        
        public CleanSheetChallenge(ChallengeRewardType challengeRewardType) : base(challengeRewardType)
        {
        }

        public override bool HasAchievedCondition()
        {
            return _numberOfCleanSheetsAchieved >= _numberOfCleanSheetsRequired;
        }
        
        public override string GetDescription()
        {
            return "Kill no enemies in " + (_numberOfCleanSheetsRequired - _numberOfCleanSheetsAchieved)
                                         + " consecutive battles";
        }

        public void EnemyKilled()
        {
            _numberOfCleanSheetsAchieved = -1;
            Debug.Log("killed");
        }

        public void EndOfBattle()
        {
            _numberOfCleanSheetsAchieved++;
            Debug.Log("ended battle");
        }
    }
}
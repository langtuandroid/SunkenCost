using Challenges.Listeners;
using UnityEngine;

namespace Challenges
{
    public class CleanSheetChallenge : Challenge, IKillListener, IEndOfBattleListener
    {
        private readonly int _numberOfCleanSheetsRequired;
        private int _numberOfCleanSheetsAchieved = 0;
        
        public CleanSheetChallenge(ChallengeRewardType challengeRewardType, int level) : base(challengeRewardType, level)
        {
            _numberOfCleanSheetsRequired = 3 + level;
        }

        public override bool HasAchievedCondition()
        {
            return _numberOfCleanSheetsAchieved >= _numberOfCleanSheetsRequired;
        }
        public override string GetDescription()
        {
            var numLeft = _numberOfCleanSheetsRequired - _numberOfCleanSheetsAchieved;
            if (HasAchievedCondition()) numLeft = _numberOfCleanSheetsRequired;
            
            return "Kill no enemies in " + 
                   numLeft + " consecutive battles";
        }

        public void EnemyKilled()
        {
            _numberOfCleanSheetsAchieved = -1;
            Debug.Log("killed");
        }

        public void EndOfBattle()
        {
            _numberOfCleanSheetsAchieved++;
        }
    }
}
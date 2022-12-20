using System.Collections.Generic;

namespace Etchings
{
    public class StrikeZoneEtching : CharPreMovementActivatedEffect
    {
        private Dictionary<ActiveEtching, StatModifier> _appliedModifiers =
            new Dictionary<ActiveEtching, StatModifier>();
        
        protected override bool CheckInfluence(int stickNum)
        {
            throw new System.NotImplementedException();
        }
        
        protected override bool TestCharAboutToMoveActivatedEffect()
        {
            var currentEnemy = ActiveEnemiesManager.current.CurrentEnemy;
            var stickNum = Stick.GetStickNumber();

            if (currentEnemy.StickNum + currentEnemy.NextMove != stickNum) return false;

            currentEnemy.stats.AddVulnerable(2);
            return true;
        }
    }
}
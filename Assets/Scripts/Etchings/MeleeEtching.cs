using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using Enemies;
using UnityEngine;

namespace Etchings
{
    public class MeleeEtching : DamageEtching
    {
        protected override bool TestCharMovementActivatedEffect(Enemy enemy)
        {
            return enemy.PlankNum == PlankNum;
        }
        
        protected override List<BattleEvent> GetDesignResponsesToEvent(BattleEvent battleEvent)
        {
            return new List<BattleEvent>() {DamageEnemy(battleEvent.enemyAffectee)};
        }
    }
}

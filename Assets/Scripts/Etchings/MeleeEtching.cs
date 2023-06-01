using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using Enemies;
using UnityEngine;

namespace Etchings
{
    public class MeleeEtching : DamageEtching
    {
        protected override bool GetIfRespondingToEnemyMovement(Enemy enemy)
        {
            return enemy.PlankNum == PlankNum;
        }

        protected override DesignResponse GetResponseToMovement(Enemy enemy)
        {
            return new DesignResponse(PlankNum, DamageEnemy(enemy));
        }
    }
}

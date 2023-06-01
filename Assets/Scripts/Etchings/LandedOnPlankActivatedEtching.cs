using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using Enemies;
using UnityEngine;

namespace Etchings
{
    public abstract class LandedOnPlankActivatedEtching : MovementActivatedEtching
    {
        protected override BattleEventType GetEventType()
        {
            return BattleEventType.EnemyMoved;
        }
    } 
}


using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using Damage;

namespace Items.Items
{
    public class ShortFuseItem : EquippedItem, IDamageMultiplierModifier
    {
        private int _turnPlayerLastLostLife = -1;

        protected override List<BattleEventResponseTrigger> GetItemResponseTriggers()
        {
            return new List<BattleEventResponseTrigger>
            {
                AddResponseTrigger(BattleEventType.PlayerLostLife, b =>
                {
                    _turnPlayerLastLostLife = Battle.Current.Turn;
                    return BattleEvent.None;
                })
            };
        }

        public bool CanModify(EnemyDamage enemyDamage)
        {
            return (Battle.Current.Turn == _turnPlayerLastLostLife);
        }

        public DamageModification GetDamageMultiplier(EnemyDamage enemyDamage)
        {
            return new DamageModification(this, Amount);
        }
    }
}
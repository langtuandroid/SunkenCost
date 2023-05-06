using System;
using System.Collections;
using BattleScreen;
using BattleScreen.BattleEvents;
using Damage;

namespace Items.Items
{
    public class ShortFuseItem : EquippedItem, IDamageMultiplierModifier
    {
        private int _turnPlayerLastLostLife = -1;

        protected override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
        {
            if (battleEvent.type == BattleEventType.PlayerLostLife)
            {
                _turnPlayerLastLostLife = Battle.Current.Turn;
                return true;
            }

            return false;
        }

        protected override BattleEventPackage GetResponse(BattleEvent battleEvent)
        {
            return new BattleEventPackage(BattleEvent.None);
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
using System;
using System.Collections;
using BattleScreen;
using Damage;

namespace Items.Items
{
    public class ShortFuseItem : EquippedItem, IDamageMultiplierModifier
    {
        private int _turnPlayerLastLostLife = -1;

        public override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
        {
            if (battleEvent.type == BattleEventType.PlayerLostLife)
                _turnPlayerLastLostLife = Battle.Current.Turn;

            return false;
        }

        protected override BattleEvent GetResponse(BattleEvent battleEvent)
        {
            throw new NotImplementedException();
        }

        public bool CanModify(BattleEvent battleEventToModify)
        {
            return (Battle.Current.Turn == _turnPlayerLastLostLife);
        }

        public DamageModification GetDamageMultiplier(BattleEvent battleEvent)
        {
            return new DamageModification(this, Amount);
        }
    }
}
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
                _turnPlayerLastLostLife = Battle.Current.Turn;

            return true;
        }

        protected override BattleEventPackage GetResponse(BattleEvent battleEvent)
        {
            return BattleEventPackage.Empty;
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
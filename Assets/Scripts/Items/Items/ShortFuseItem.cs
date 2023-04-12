using System.Collections;
using BattleScreen;
using BattleScreen.BattleEvents;
using BattleScreen.BattleEvents.EventTypes;
using Damage;

namespace Items.Items
{
    public class ShortFuseItem : EquippedItem, IDamageMultiplierModifier
    {
        private int _turnPlayerLastLostLife = -1;

        public override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
        {
            return battleEvent.Type == BattleEventType.PlayerLostLife;
        }

        protected override BattleEvent GetResponse(BattleEvent battleEvent)
        {
            _turnPlayerLastLostLife = Battle.Current.Turn;
            return new BattleEvent(BattleEventType.GenericItemActivation);
        }

        public bool CanModify(EnemyDamageBattleEvent enemyDamageToModify)
        {
            return (Battle.Current.Turn == _turnPlayerLastLostLife);
        }

        public DamageModification GetDamageMultiplier(EnemyDamageBattleEvent enemyDamageToModify)
        {
            return new DamageModification(this, Amount);
        }
    }
}
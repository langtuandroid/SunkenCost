using System.Collections;
using BattleScreen;
using Enemies;
using Etchings;
using EventListeners;
using UnityEngine;

namespace Items.Items
{
    public class ShortFuseItem : EquippedItem, IBattleEventResponder, IDamageMultiplierModifier
    {
        private int _turnPlayerLastLostLife = -1;

        public bool GetResponseToBattleEvent(BattleEvent previousBattleEvent)
        {
            return previousBattleEvent.battleEventType == BattleEventType.PlayerLostLife;
        }

        public IEnumerator ExecuteResponseToAction(BattleEvent battleEvent)
        {
            _turnPlayerLastLostLife = BattleManager.Current.Turn;
            yield break;
        }

        public bool CanModify(DamageBattleEvent damageToModify)
        {
            return (BattleManager.Current.Turn == _turnPlayerLastLostLife);
        }

        public int GetDamageMultiplier(DamageBattleEvent damageToModify)
        {
            return Amount;
        }
    }
}
using System;
using System.Collections;
using BattleScreen;
using BattleScreen.BattleEvents;

namespace Items.Items
{
    public class PassiveIncomeItem : EquippedItem
    {
        private bool _hasKilledEnemyThisBattle = false;

        protected override bool GetIfRespondingToBattleEvent(BattleEvent battleEvent)
        {
            if (battleEvent.type == BattleEventType.EnemyKilled)
                _hasKilledEnemyThisBattle = true;
            
            return battleEvent.type == BattleEventType.EndedBattle && !_hasKilledEnemyThisBattle;
        }
        
        protected override BattleEventPackage GetResponse(BattleEvent battleEvent)
        {
            return new BattleEventPackage(new BattleEvent(BattleEventType.TryGainedGold) {modifier = Amount});
        }
    }
}
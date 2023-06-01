using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;

namespace Items.Items
{
    public class PassiveIncomeItem : EquippedItem
    {
        private bool _hasKilledEnemyThisBattle = false;

        protected override List<BattleEventResponseTrigger> GetItemResponseTriggers()
        {
            return new List<BattleEventResponseTrigger>
            {
                EventResponseTrigger(BattleEventType.EnemyKilled, b =>
                {
                    _hasKilledEnemyThisBattle = true;
                    return BattleEvent.None;
                }),
                PackageResponseTrigger(BattleEventType.EndedBattle, b => GainGold(), 
                    b => !_hasKilledEnemyThisBattle)
            };
        }

        private BattleEventPackage GainGold()
        {
            return new BattleEventPackage(new BattleEvent(BattleEventType.TryGainedGold) {modifier = Amount});
        }
    }
}
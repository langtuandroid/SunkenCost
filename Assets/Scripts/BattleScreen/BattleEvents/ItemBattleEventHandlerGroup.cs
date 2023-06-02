using System;
using BattleScreen.BattleEvents;
using UnityEngine;

namespace BattleScreen
{
    public class ItemBattleEventHandlerGroup : BattleEventHandlerGroup
    {
        private void Start()
        {
            foreach (var item in RunProgress.Current.ItemInventory.AllItemsAsBattleEventResponders)
            {
                CreateHandler(item);
            }
        }

        public override BattleEventPackage GetNextResponse(BattleEvent previousBattleEvent)
        {
            return base.GetNextResponse(previousBattleEvent);
        }
    }
}

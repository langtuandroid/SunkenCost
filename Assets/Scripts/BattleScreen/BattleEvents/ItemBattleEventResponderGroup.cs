using System;
using BattleScreen.BattleEvents;
using UnityEngine;

namespace BattleScreen
{
    public class ItemBattleEventResponderGroup : BattleEventResponderGroup
    {
        private void Start()
        {
            foreach (var item in RunProgress.Current.ItemInventory.AllItemsAsBattleEventResponders)
            {
                AddResponder(item);
            }
        }

        public override BattleEventPackage GetNextResponse(BattleEvent previousBattleEvent)
        {
            return base.GetNextResponse(previousBattleEvent);
        }
    }
}

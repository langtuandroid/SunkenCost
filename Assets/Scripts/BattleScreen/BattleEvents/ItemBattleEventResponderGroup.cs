using System;
using BattleScreen.BattleEvents;
using UnityEngine;

namespace BattleScreen
{
    public class ItemBattleEventResponderGroup : BattleEventResponderGroup
    {
        private void Start()
        {
            foreach (var item in RunProgress.ItemInventory.AllItemsAsBattleEventResponders)
            {
                AddResponder(item);
            }
        }

        public override BattleEventPackage GetNextResponse(BattleEvent battleEventToRespondTo)
        {
            return base.GetNextResponse(battleEventToRespondTo);
        }
    }
}

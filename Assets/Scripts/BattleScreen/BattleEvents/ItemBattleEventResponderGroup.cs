using System;
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
    }
}

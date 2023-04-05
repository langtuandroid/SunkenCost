using System;
using UnityEngine;

namespace BattleScreen
{
    public class ItemBattleEventResponderGroup : BattleEventResponderGroup
    {
        private void Awake()
        {
            foreach (var item in RunProgress.ItemInventory.AllItemsAsBattleActionResponders)
            {
                Add(item);
            }
        }
    }
}

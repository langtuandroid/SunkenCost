using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using BattleScreen.BattleEvents;
using UI;
using UnityEngine;

namespace Items.Items
{
    public abstract class EquippedItem : BattleEventHandler
    {
        public ItemInstance ItemInstance { get; private set; }
        protected int Amount => ItemInstance.modifier;
        
        
        public void SetInstance(ItemInstance itemInstance)
        {
            ItemInstance = itemInstance;
        }

        public override List<BattleEventResponseTrigger> GetBattleEventResponseTriggers()
        {
            return GetItemResponseTriggers().Select(r => 
                AddResponseTrigger(r.battleEventType, b => ActivateItem(b, r.response), r.condition)).ToList();
        }

        private BattleEventPackage ActivateItem(BattleEvent battleEvent, Func<BattleEvent, BattleEventPackage> responseFunc)
        {
            ItemIconsDisplay.Current.ActivateItemDisplay(ItemInstance);
            return responseFunc.Invoke(battleEvent).WithIdentifier(BattleEventType.ItemActivated, ResponderID);
        }

        protected virtual List<BattleEventResponseTrigger> GetItemResponseTriggers()
        {
            return new List<BattleEventResponseTrigger>();
        }
    }
}
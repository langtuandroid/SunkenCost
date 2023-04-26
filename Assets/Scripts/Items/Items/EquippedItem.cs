using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using UI;
using UnityEngine;

namespace Items.Items
{
    public abstract class EquippedItem : BattleEventResponder
    {
        public ItemInstance ItemInstance { get; private set; }
        protected int Amount => ItemInstance.modifier;
        
        
        public void SetInstance(ItemInstance itemInstance)
        {
            ItemInstance = itemInstance;
        }

        public override BattleEventPackage GetResponseToBattleEvent(BattleEvent previousBattleEvent)
        {
            if (GetIfRespondingToBattleEvent(previousBattleEvent))
            {
                ItemIconsDisplay.Current.ActivateItemDisplay(ItemInstance);
                return GetResponse(previousBattleEvent);
            }
            
            return BattleEventPackage.Empty;
        }

        protected abstract bool GetIfRespondingToBattleEvent(BattleEvent previousBattleEvent);
        protected abstract BattleEventPackage GetResponse(BattleEvent battleEvent);
    }
}
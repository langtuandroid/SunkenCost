using System.Collections;
using BattleScreen;
using BattleScreen.BattleEvents;
using BattleScreen.Events;
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
        
        public override IEnumerator DisplayEvent(BattleEvent battleEvent)
        {
            // Glow the icon
            ItemIconsDisplay.ActivateItemDisplay(ItemInstance);
            yield return new WaitForSeconds(BattleEventsManager.ActionExecutionSpeed);
        }
    }
}
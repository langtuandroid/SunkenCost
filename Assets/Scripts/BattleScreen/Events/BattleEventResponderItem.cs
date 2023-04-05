using System;
using System.Collections;
using BattleScreen;
using Items.Items;
using UI;
using UnityEngine;

namespace Items
{
    public abstract class BattleEventResponderItem : EquippedItem, IBattleEventResponder
    {
        public abstract bool GetResponseToBattleEvent(BattleEvent previousBattleEvent);
        
        public IEnumerator ExecuteResponseToAction(BattleEvent battleEvent)
        {
            // Glow the icon
            ItemIconsDisplay.ActivateItemDisplay(ItemInstance);
            yield return new WaitForSeconds(BattleEvents.ActionExecutionSpeed);
            
            // Wait for the action to finish
            yield return StartCoroutine(Activate(battleEvent));
        }

        protected abstract IEnumerator Activate(BattleEvent battleEvent);
    }
}

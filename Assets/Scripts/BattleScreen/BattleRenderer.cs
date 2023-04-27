using System.Collections.Generic;
using System.Linq;
using BattleScreen.BattleEvents;
using UnityEngine;

namespace BattleScreen
{
    public class BattleRenderer : MonoBehaviour
    {
        public static BattleRenderer Current;

        private List<IBattleEventUpdatedUI> _eventRespondingUI = new List<IBattleEventUpdatedUI>();

        private void Awake()
        {
            if (Current)
                Destroy(Current.gameObject);

            Current = this;
        }

        public void RenderEventPackage(BattleEventPackage battleEventPackage)
        {
            foreach (var battleEvent in battleEventPackage.battleEvents)
            {
                foreach (var respondingUI in _eventRespondingUI)
                {
                    respondingUI.RespondToBattleEvent(battleEvent);
                }
            }
        }

        public void RegisterUIUpdater(IBattleEventUpdatedUI ui)
        {
            _eventRespondingUI.Add(ui);
        }
        
        public void DeregisterUIUpdater(IBattleEventUpdatedUI ui)
        {
            _eventRespondingUI.Remove(ui);
        }
    }
}
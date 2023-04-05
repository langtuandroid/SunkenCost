using System.Collections;
using BattleScreen;

namespace Items.Items
{
    public class ConfusedItem : BattleEventResponderItem, IHasPickupAction, IBattleEventResponder
    {
        private StatModifier _extraPlank;

        public void OnPickup()
        {
            _extraPlank = new StatModifier(Amount, StatModType.Flat);
            RunProgress.PlayerStats.maxPlanks.AddModifier(_extraPlank);
        }
        
        public void OnDestroy()
        {
            RunProgress.PlayerStats.maxPlanks.RemoveModifier(_extraPlank);
        }

        public override bool GetResponseToBattleEvent(BattleEvent previousBattleEvent)
        {
            return previousBattleEvent.battleEventType == BattleEventType.StartBattle;
        }
        
        protected override IEnumerator Activate(BattleEvent battleEvent)
        {
            StickManager.current.RandomisePlanks();
            yield break;
        }
    }
}
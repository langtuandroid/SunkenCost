using BattleScreen;
using BattleScreen.BattleEvents;

namespace Items.Items
{
    public class BellsAndWhistlesItem : ExtraPlankItem
    {
        public override void OnPickup()
        {
            RunProgress.PlayerStats.EnemyMovementModifier += 2;
            base.OnPickup();
        }

        public override void OnDestroy()
        {
            RunProgress.PlayerStats.EnemyMovementModifier -= 2;
            base.OnDestroy();
        }

        protected override bool GetIfRespondingToBattleEvent(BattleEvent previousBattleEvent)
        {
            return false;
        }

        protected override BattleEventPackage GetResponse(BattleEvent battleEvent)
        {
            throw new System.NotImplementedException();
        }
    }
}
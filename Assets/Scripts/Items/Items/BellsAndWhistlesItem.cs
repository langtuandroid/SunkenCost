using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;

namespace Items.Items
{
    public class BellsAndWhistlesItem : ExtraPlankItem
    {
        public override void OnPickup()
        {
            RunProgress.Current.PlayerStats.EnemyMovementModifier += 2;
            base.OnPickup();
        }

        public override void OnDestroy()
        {
            RunProgress.Current.PlayerStats.EnemyMovementModifier -= 2;
            base.OnDestroy();
        }
    }
}
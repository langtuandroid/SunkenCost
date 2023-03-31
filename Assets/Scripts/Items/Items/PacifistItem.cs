using EventListeners;

namespace Items.Items
{
    public class PacifistItem : EquippedItem, IEndOfBattleListener, IKillListener
    {
        private bool _hasKilledEnemyThisBattle = false;
        
        public void EndOfBattle()
        {
            if (!_hasKilledEnemyThisBattle)
            {
                RunProgress.PlayerStats.AlterGold(Amount);
            }
        }

        public void EnemyKilled()
        {
            _hasKilledEnemyThisBattle = true;
        }
    }
}
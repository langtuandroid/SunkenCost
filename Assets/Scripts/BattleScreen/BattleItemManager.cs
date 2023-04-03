using UnityEngine;

namespace BattleScreen
{
    public class BattleItemManager : MonoBehaviour
    {
        private void Start()
        {
            BattleEvents.Current.OnStartBattle += StartOfBattle;
            BattleEvents.Current.OnEnemyAttacked += EnemyAttacked;
            BattleEvents.Current.OnEnemyHealed += EnemyHealed;
            BattleEvents.Current.OnEnemyReachedEnd += EnemyReachedEnd;
            BattleEvents.Current.OnPlayerLostLife += PlayerLostLife;
            BattleEvents.Current.OnPlayerGainedGold += PlayerGainedGold;
        }

        private void StartOfBattle()
        {
            foreach (var listener in RunProgress.ItemInventory.StartOfBattleListeners)
                listener.StartOfBattle();
        }

        private void EnemyAttacked()
        {
            foreach (var listener in RunProgress.ItemInventory.EnemyAttackedListeners)
                listener.EnemyAttacked();
        }

        private void EnemyHealed()
        {
            foreach (var listener in RunProgress.ItemInventory.EnemyHealedListeners)
                listener.EnemyHealed();
        }

        private void EnemyReachedEnd()
        {
            foreach (var listener in RunProgress.ItemInventory.EnemyReachedEndListeners)
                listener.EnemyReachedEnd();
        }

        private void PlayerLostLife()
        {
            foreach (var listener in RunProgress.ItemInventory.PlayerLostLifeListeners)
                listener.PlayerPlayerLostLife();
        }

        private void PlayerGainedGold()
        {
            foreach (var listener in RunProgress.ItemInventory.PlayerGainedGoldListeners)
                listener.GainedGold();
        }
    }
}

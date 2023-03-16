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
    }
}

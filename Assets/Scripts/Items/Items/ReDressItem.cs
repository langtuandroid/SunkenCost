using System.Collections;
using BattleScreen;
namespace Items.Items
{
    public class ReDressItem : BattleEventResponderItem
    {
        public override bool GetResponseToBattleEvent(BattleEvent previousBattleEvent)
        {
            return previousBattleEvent.battleEventType == BattleEventType.EnemyReachedBoat;
        }

        protected override IEnumerator Activate(BattleEvent battleEvent)
        {
            var enemies = ActiveEnemiesManager.Current.ActiveEnemies;
            for (var i = enemies.Count - 1 ; i > 0; i--)
            {
                if (!enemies[i] || enemies[i].IsDestroyed) continue;
                yield return StartCoroutine(
                    BattleState.current.DamageHandler.DamageEnemy(Amount, enemies[i], DamageSource.Item));
            }
        }
    }
}
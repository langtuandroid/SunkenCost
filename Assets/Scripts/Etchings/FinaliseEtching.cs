using System.Linq;
using UnityEngine;

namespace Etchings
{
    public class FinaliseEtching : DamageEtching
    {
        protected override bool CheckInfluence(int stickNum)
        {
            return stickNum == Stick.GetStickNumber();
        }

        protected override bool TestCharMovementActivatedEffect()
        {
            var currentEnemy = ActiveEnemiesManager.CurrentEnemy;

            if (!CheckInfluence(currentEnemy.StickNum)) return false;

            var enemiesOnPlanks = ActiveEnemiesManager.Current.ActiveEnemies.Where
                (e => e.StickNum != 0).ToArray();
            foreach (var enemy in enemiesOnPlanks)
            {
                if (enemy.StickNum > 0) enemy.Stick.SetTempColour(design.Color);
                DamageEnemy(enemy);
            }

            if (enemiesOnPlanks.Any(e => !e.IsDestroyed))
            {
                PlayerController.current.TakeLife();
            }
            
            return true;
        }
    }
}

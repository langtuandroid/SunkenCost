using System.Linq;
using UnityEngine;

namespace Etchings
{
    public class GrandFinalistEtching : DamageEtching
    {
        protected override bool CheckInfluence(int stickNum)
        {
            return stickNum == Stick.GetStickNumber();
        }

        protected override bool TestCharMovementActivatedEffect()
        {
            var enemy = ActiveEnemiesManager.CurrentEnemy;

            if (!CheckInfluence(enemy.StickNum)) return false;

            var activeEnemies = ActiveEnemiesManager.Current.ActiveEnemies.ToList();
            foreach (var activeEnemy in activeEnemies)
            {
                if (activeEnemy.StickNum > 0) activeEnemy.Stick.SetTempColour(design.Color);
                DamageEnemy(activeEnemy);
            }

            if (ActiveEnemiesManager.Current.ActiveEnemies.Count > 0)
            {
                Debug.Log(ActiveEnemiesManager.Current.ActiveEnemies.Count);
                PlayerController.current.TakeLife();
            }
            
            return true;
        }
    }
}

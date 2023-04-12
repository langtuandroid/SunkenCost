using Enemies;
using UnityEngine;

namespace Etchings
{
    public class MeleeEtching : DamageEtching
    {
        protected override bool TestCharMovementActivatedEffect()
        {
            var enemy = ActiveEnemiesManager.CurrentEnemy;

            if (!CheckInfluence(enemy.StickNum)) return false;
            
            DamageEnemy(enemy);
            return true;

        }

        protected override bool CheckInfluence(int stickNum)
        {
            return stickNum == Plank.GetPlankNum();
        }

        protected override void DamageEnemy(Enemy enemy)
        {
            Plank.SetTempColour(design.Color);
            UsesUsedThisTurn += 1;
            base.DamageEnemy(enemy);
            InGameSfxManager.current.DamagedEnemy();
        }
        
    }
}

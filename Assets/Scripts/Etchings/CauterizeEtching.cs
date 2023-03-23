using UnityEngine;

namespace Etchings
{
    public class CauterizeEtching : MeleeEtching
    {
        protected override bool TestCharMovementActivatedEffect()
        {
            var enemy = ActiveEnemiesManager.CurrentEnemy;
            if (!CheckInfluence(enemy.StickNum)) return false;

            var poison = enemy.stats.Poison;

            if (poison <= 0) return false;
            
            var newStatMod = new StatModifier
                (-poison * GetStatValue(St.MaxHealthMultiplier), StatModType.Flat);
            enemy.AddMaxHealthModifier(newStatMod);

            StartCoroutine(ColorForActivate());
            return true;

        }
    }
}
using UnityEngine;

namespace Etchings
{
    public class ResearchEtching : CharMovementActivatedEtching
    {
        protected override bool CheckInfluence(int stickNum)
        {
            return stickNum == Stick.GetStickNumber();
        }

        protected override bool TestCharMovementActivatedEffect()
        {
            var enemy = ActiveEnemiesManager.CurrentEnemy;
            if (!CheckInfluence(enemy.StickNum)) return false;
            StartCoroutine(ColorForActivate());

            var amountToHeal = enemy.MaxHealth.Value - enemy.Health;
                
            enemy.Heal(amountToHeal);

            var timesMetRequirement = (int)Mathf.Floor(((float)amountToHeal / GetStatValue(St.IntRequirement)));
            var amountOfGoldToGive = timesMetRequirement * GetStatValue(St.Gold);
            BattleManager.Current.AlterGold(amountOfGoldToGive);

            return true;

        }
    }
}
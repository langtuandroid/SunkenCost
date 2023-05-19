using System.Collections;
using BattleScreen;
using BattleScreen.BattleEvents;
using Enemies;
using UnityEngine;

namespace Damage
{
    public class DamageVisualiser : MonoBehaviour
    {
        [SerializeField] private DamageText _damageText;

        public void Damage(int baseDamage, DamageModificationPackage damageModificationPackage)
        {
            StartCoroutine(VisualiseDamage(baseDamage, damageModificationPackage));
        }

        public void Heal(int amount)
        {
            StartCoroutine(VisualiseHeal(amount));
        }

        public IEnumerator VisualiseHeal(int amount)
        {
            _damageText.Heal(amount);

            yield return new WaitForSecondsRealtime(Battle.ActionExecutionSpeed / 2);
            _damageText.Hide();
        }

        private IEnumerator VisualiseDamage(int baseDamage, DamageModificationPackage damageModificationPackage)
        {
            _damageText.Damage(baseDamage, damageModificationPackage);

            yield return new WaitForSecondsRealtime(Battle.ActionExecutionSpeed / 2);
            _damageText.Hide();
        }
    }
}
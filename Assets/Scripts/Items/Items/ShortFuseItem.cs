using Enemies;
using Etchings;
using EventListeners;
using UnityEngine;

namespace Items.Items
{
    public class ShortFuseItem : EquippedItem, IPlayerLostLifeListener, IDamageMultiplierModifier
    {
        private int _turnPlayerLastLostLife = -1;
        
        public void PlayerPlayerLostLife()
        {
            _turnPlayerLastLostLife = BattleManager.Current.Turn;
        }
        
        public int GetDamageModification(int damage, Enemy enemy, DamageSource source, Etching etching = null)
        {
            if (BattleManager.Current.Turn == _turnPlayerLastLostLife)
            {
                Debug.Log(Amount);
                return damage * Amount;
            }

            return damage;
        }
    }
}
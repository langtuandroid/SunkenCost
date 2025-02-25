using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using BattleScreen.BattleEvents;
using Enemies;
using Etchings;
using Items.Items;
using UnityEngine;

namespace Damage
{
    public enum DamageSource 
    {
        None,
        Etching,
        Poison,
        Item,
        Self,
        EnemyAbility,
        Boat,
        PlankDestruction
    }

    public interface IDamageFlatModifier
    {
        public bool CanModify(EnemyDamage enemyDamage);
    
        public DamageModification GetDamageAddition(EnemyDamage enemyDamage);
    }
    
    public interface IDamageMultiplierModifier
    {
        public bool CanModify(EnemyDamage enemyDamage);
        public DamageModification GetDamageMultiplier(EnemyDamage enemyDamage);
    }

    public struct EnemyDamage
    {
        public int baseDamage;
        public Enemy affectedEnemy;
        public DamageSource source;
        public Enemy affectingEnemy;
    }

    public static class DamageHandler
    {
        public static BattleEvent DamageEnemy(int directDamage, int enemyResponderID, DamageSource source, Enemy enemyDamaging = null)
        {
            var damage = new EnemyDamage()
            {
                baseDamage = directDamage,
                affectedEnemy = BattleEventResponseSequencer.Current.GetEnemyByResponderID(enemyResponderID),
                source = source,
                affectingEnemy = enemyDamaging
            };
            

            var damageModifications = 
                BattleEventResponseSequencer.Current.GetDamageModifiers(damage);

            var flatTotal = directDamage + 
                            damageModifications.flatModifications.Sum(d => d.modificationAmount);
        
            var multiTotal =
                damageModifications.multiModifications.Aggregate
                    (flatTotal, (current, mod) => mod.modificationAmount * current);

            var damageEvent = new BattleEvent(BattleEventType.EnemyAttacked)
            {
                modifier = multiTotal,
                primaryResponderID = enemyResponderID,
                source = source,
                damageModificationPackage = damageModifications
            };

            if (enemyDamaging is not null)
                damageEvent.secondaryResponderID = enemyDamaging.ResponderID;

            return damageEvent;
        }
    }
}
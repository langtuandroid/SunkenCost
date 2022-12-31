using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using Etchings;
using UnityEngine;

public enum DamageSource 
{
    Plank,
    Poison,
    Item,
    Self,
}

public interface IDamageFlatModifier
{
    public int GetDamageModification(int damage, Enemy enemy, DamageSource source, Etching etching = null);
}
    
public interface IDamageMultiplierModifier
{
    public int GetDamageModification(int damage, Enemy enemy, DamageSource source, Etching etching = null);
}

public static class DamageHandler
{
    public static void DamageEnemy(int directDamage, Enemy enemy, DamageSource source, Etching etching = null)
    {
        var damage = directDamage;
        
        /* DAMAGE SEQUENCE
         
         - Initial source
         - Flat etching modifiers (left to right)
         - Flat booty modifiers
         - Multiplier etching modifiers (left to right except plank the enemy is on - IF APPLICABLE - then the enemy's plank)
         - Multiplier booty modifiers
         
         */

        var flatModifyingEtchings = EtchingManager.Current.etchingOrder.OfType<IDamageFlatModifier>();
        var multiModifyingEtchings = EtchingManager.Current.etchingOrder.OfType<IDamageMultiplierModifier>();
        var flatModifyingItems = BattleItemManager.ActiveItems.OfType<IDamageFlatModifier>();
        var multiModifyingItems = BattleItemManager.ActiveItems.OfType<IDamageMultiplierModifier>();
        
        damage = flatModifyingEtchings.Aggregate(damage, (current, mod) => mod.GetDamageModification(current, enemy, source, etching));
        damage = flatModifyingItems.Aggregate(damage, (current, mod) => mod.GetDamageModification(current, enemy, source, etching));
        IDamageMultiplierModifier enemyStickEtching = null;
        
        foreach (var etch in multiModifyingEtchings)
        {
            var etchAsEtch = etch as Etching;
            if (enemy.StickNum == etchAsEtch.Stick.GetStickNumber())
            {
                enemyStickEtching = etch;
                continue;
            }
            
            damage = etch.GetDamageModification(damage, enemy, source, etching);
        }
        
        if (enemyStickEtching != null)
        {
            damage = enemyStickEtching.GetDamageModification(damage, enemy, source, etching);
        }
        
        
        damage = multiModifyingItems.Aggregate(damage, (current, item) => item.GetDamageModification(current, enemy, source, etching));

        enemy.TakeDamage(damage);
        
        if (source == DamageSource.Plank)
        {
            BattleEvents.Current.EnemyAttacked(enemy, etching);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using BattleScreen.BattleEvents;
using BattleScreen.BattleEvents.EventTypes;
using BattleScreen.Events;
using Damage;
using Enemies;
using Etchings;
using Items.Items;
using UnityEngine;

public enum DamageSource 
{
    Etching,
    Poison,
    Item,
    Self,
    EnemyAbility,
    Boat
}

public enum DamageModificationType
{
    Flat,
    Multi
}
public interface IDamageFlatModifier
{
    public bool CanModify(EnemyDamageBattleEvent enemyDamageToModify);
    
    public DamageModification GetDamageAddition(EnemyDamageBattleEvent enemyDamageToModify);
}
    
public interface IDamageMultiplierModifier
{
    public bool CanModify(EnemyDamageBattleEvent enemyDamageToModify);
    public DamageModification GetDamageMultiplier(EnemyDamageBattleEvent enemyDamageToModify);
}

public static class DamageHandler
{
    public static BattleEvent DamageEnemy(int directDamage, Enemy enemy, DamageSource source, 
        Etching etching = null, EquippedItem item = null, Enemy enemyDamaging = null)
    {
        var damage = directDamage;

        var preModDamageBattleAction = GetNewDamageBattleAction
            (directDamage, enemy, DamageModificationPackage.Empty(), source, etching, item, enemy);

        var damageModifications = 
            BattleEventsManager.Current.GetDamageModifiers(preModDamageBattleAction);

        var flatTotal = damageModifications.flatModifications.Sum(d => d.modificationAmount);
        var multiTotal = new int[]
        {
            damageModifications.multiModifications.Aggregate
                (flatTotal, (current, mod) => mod.modificationAmount * current)
        };

        Debug.Log("Base damage: " + directDamage);
        Debug.Log("Flat modifiers: " + flatTotal);
        Debug.Log("Multi modifiers:");

        var total = directDamage + flatTotal;

        foreach (var multi in multiTotal)
        {
            Debug.Log(multi);
            total *= multi;
        }

        Debug.Log("Total " + total);
        
        enemy.TakeDamage(total, source);
        
        return  GetNewDamageBattleAction
            (total, enemy, damageModifications, source, etching, item, enemy);
    }

    private static EnemyDamageBattleEvent GetNewDamageBattleAction(int directDamage, Enemy enemy, 
        DamageModificationPackage damageModificationPackage, DamageSource source, Etching etching = null, 
        EquippedItem item = null, Enemy enemyDamaging = null)
    {
        return source switch
        {
            DamageSource.Etching => new EtchingEnemyDamageBattleEvent
                (enemy, directDamage, damageModificationPackage, etching),
            DamageSource.Poison => new EnemyDamageBattleEvent
                (enemy, directDamage, damageModificationPackage, DamageSource.Poison),
            DamageSource.Item => new ItemEnemyDamageBattleEvent(enemy, directDamage, damageModificationPackage, item),
            DamageSource.Self => new EnemyAbilityEnemyDamageBattleEvent
                (enemy, directDamage, damageModificationPackage, enemy),
            DamageSource.EnemyAbility => new EnemyAbilityEnemyDamageBattleEvent
                (enemy, directDamage, damageModificationPackage, enemyDamaging),
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        };
    }
}
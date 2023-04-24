using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen;
using BattleScreen.BattleEvents;
using Damage;
using Enemies;
using Etchings;
using Items.Items;
using UnityEngine;

public enum DamageSource 
{
    None,
    Etching,
    Poison,
    Item,
    Self,
    EnemyAbility,
    Boat,
}

public interface IDamageFlatModifier
{
    public bool CanModify(BattleEvent battleEventToModify);
    
    public DamageModification GetDamageAddition(BattleEvent battleEventToModify);
}
    
public interface IDamageMultiplierModifier
{
    public bool CanModify(BattleEvent battleEventToModify);
    public DamageModification GetDamageMultiplier(BattleEvent battleEventToModify);
}

public static class DamageHandler
{
    public static BattleEvent DamageEnemy(int directDamage, Enemy enemy, DamageSource source,
        Etching etching = null, EquippedItem item = null, Enemy enemyDamaging = null)
    {
        var damage = directDamage;

        var battleResponder = source switch
        {
            DamageSource.None => throw new ArgumentException(),
            DamageSource.Etching => etching as BattleEventResponder,
            DamageSource.Poison => enemy as BattleEventResponder,
            DamageSource.Item => item as BattleEventResponder,
            DamageSource.Self => enemy as BattleEventResponder,
            DamageSource.EnemyAbility => enemyDamaging as BattleEventResponder,
            DamageSource.Boat => null,
            _ => throw new ArgumentException()
        };

        var preModDamageBattleAction = new BattleEvent(BattleEventType.EnemyDamaged, battleResponder)
        {
            modifier = damage,
            enemyAffectee = enemy,
            damageSource = source,
            damageModificationPackage = DamageModificationPackage.Empty(),
            etching = etching,
            item = item,
            enemyAffector = enemyDamaging
        };

        var damageModifications = 
            BattleEventsManager.Current.GetDamageModifiers(preModDamageBattleAction);

        var flatTotal = directDamage + 
                        damageModifications.flatModifications.Sum(d => d.modificationAmount);
        
        var multiTotal =
            damageModifications.multiModifications.Aggregate
                (flatTotal, (current, mod) => mod.modificationAmount * current);

        Debug.Log("Base damage: " + directDamage);
        Debug.Log("Total damage: " + multiTotal);
        
        enemy.TakeDamage(multiTotal, source);

        return new BattleEvent(BattleEventType.EnemyDamaged, battleResponder)
        {
            modifier = multiTotal,
            enemyAffectee = enemy,
            damageSource = source,
            damageModificationPackage = damageModifications,
            etching = etching,
            item = item,
            enemyAffector = enemyDamaging
        };
    }
}
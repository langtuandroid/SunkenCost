using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen;
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
    EnemyAbility
}

public interface IDamageFlatModifier
{
    public DamageBattleAction GetDamageModification(DamageBattleAction damageToModify);
}
    
public interface IDamageMultiplierModifier
{
    public DamageBattleAction GetDamageModification(DamageBattleAction damageToModify);
}

public class DamageHandler : MonoBehaviour
{
    public IEnumerator DamageEnemy(int directDamage, Enemy enemy, DamageSource source, 
        Etching etching = null, EquippedItem item = null, Enemy enemyDamaging = null)
    {
        var damage = directDamage;

        var preModDamageBattleAction = GetNewDamageBattleAction
            (directDamage, enemy, source, etching, item, enemy);

        var damageModifications = 
            BattleState.current.BattleActions.GetDamageModifiers(preModDamageBattleAction);

        var flatTotal = damageModifications.flatModifications.Sum(d => d.magnitude);
        var multiTotal = damageModifications.multiModifications.Sum(d => d.magnitude);

        Debug.Log("Base damage: " + directDamage);
        Debug.Log("Flat modifiers " + flatTotal);
        Debug.Log("Multi modifiers " + multiTotal);

        var totalDamage = (directDamage + flatTotal) * multiTotal;
        
        Debug.Log("Total " + totalDamage);
        
        enemy.TakeDamage(totalDamage, source);
        
        var postModDamageBattleAction = GetNewDamageBattleAction
            (totalDamage, enemy, source, etching, item, enemy);
        
        yield return BattleState.current.BattleActions.ExecuteTurnActionResponses(postModDamageBattleAction);
    }

    private DamageBattleAction GetNewDamageBattleAction(int directDamage, Enemy enemy, DamageSource source, 
        Etching etching = null, EquippedItem item = null, Enemy enemyDamaging = null)
    {
        return source switch
        {
            DamageSource.Etching => new EtchingDamageBattleAction(enemy, directDamage, etching),
            DamageSource.Poison => new DamageBattleAction(enemy, directDamage, DamageSource.Poison),
            DamageSource.Item => new ItemDamageBattleAction(enemy, directDamage, item),
            DamageSource.Self => new EnemyAbilityDamageBattleAction(enemy, directDamage, enemy),
            DamageSource.EnemyAbility => new EnemyAbilityDamageBattleAction(enemy, directDamage, enemyDamaging),
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        };
    }
}
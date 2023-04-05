using System;
using System.Collections;
using Enemies;
using Etchings;
using Items.Items;

namespace BattleScreen
{
    public enum BattleEventType
    {
        None,
        StartBattle,
        StartTurn,
        EndTurn,
        EndBattle,
        EnemyStartTurn,
        EnemyMove,
        EnemyDebuff,
        EnemyAbility,
        EnemyHeal,
        EnemyDamage,
        EnemyKilled,
        EnemyReachedBoat,
        EtchingActivation,
        ItemActivation,
        GainGold,
        PlayerLostLife,
        PlayerGainedLife,
    }

    public class BattleEvent
    {
        public readonly BattleEventType battleEventType;
        public BattleEvent(BattleEventType battleEventType)
        {
            this.battleEventType = battleEventType;
        }
    }

    public class EnemyBattleEvent : BattleEvent
    {
        public readonly Enemy enemy;

        public EnemyBattleEvent(BattleEventType battleEventType, Enemy enemy)
        : base(battleEventType)
        {
            this.enemy = enemy;
        }
    }
    
    public class DamageBattleEvent : EnemyBattleEvent
    {
        public readonly int magnitude;
        public readonly DamageSource damageSource;
        public DamageBattleEvent(Enemy enemy, int magnitude, DamageSource damageSource)
            : base(BattleEventType.EnemyDamage, enemy)
        {
            this.magnitude = magnitude;
            this.damageSource = damageSource;
        }
    }
    
    public class EnemyAbilityDamageBattleEvent : DamageBattleEvent
    {
        public readonly Enemy enemyDealingDamage;
        
        public EnemyAbilityDamageBattleEvent(Enemy enemy, int magnitude, Enemy enemyDealingDamage) : base(enemy, magnitude, DamageSource.EnemyAbility)
        {
            this.enemyDealingDamage = enemyDealingDamage;
        }
    }

    public class EtchingDamageBattleEvent : DamageBattleEvent
    {
        public readonly Etching etching;
        
        public EtchingDamageBattleEvent(Enemy enemy, int magnitude, Etching etching) : base(enemy, magnitude, DamageSource.Etching)
        {
            this.etching = etching;
        }
    }
    
    public class ItemDamageBattleEvent : DamageBattleEvent
    {
        public readonly EquippedItem item;
        
        public ItemDamageBattleEvent(Enemy enemy, int magnitude, EquippedItem item) : base(enemy, magnitude, DamageSource.Item)
        {
            this.item = item;
        }
    }
}
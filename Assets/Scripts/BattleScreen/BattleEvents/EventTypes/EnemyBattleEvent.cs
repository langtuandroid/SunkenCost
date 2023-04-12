using Damage;
using Enemies;
using Etchings;
using Items.Items;

namespace BattleScreen.BattleEvents.EventTypes
{
    public class EnemyBattleEvent : BattleEvent
    {
        public readonly Enemy enemy;

        public EnemyBattleEvent(BattleEventType battleEventType, Enemy enemy)
            : base(battleEventType) => this.enemy = enemy;
    }

    public class EnemyHealBattleEvent : EnemyBattleEvent
    {
        public readonly int healAmount;

        public EnemyHealBattleEvent(Enemy enemy, int healAmount) : base(BattleEventType.EnemyHealed, enemy) =>
            this.healAmount = healAmount;
    }

    public class EnemyPoisonBattleEvent : EnemyBattleEvent
    {
        public readonly int poisonAmount;
        
        public EnemyPoisonBattleEvent(Enemy enemy, int poisonAmount) : base(BattleEventType.EnemyPoisoned, enemy) =>
            this.poisonAmount = poisonAmount;
    }

    public class EnemyKillBattleEvent : EnemyBattleEvent
    {
        public readonly DamageSource killSource;

        public EnemyKillBattleEvent(Enemy enemy, DamageSource killSource) : base(BattleEventType.EnemyKilled, enemy) =>
            this.killSource = killSource;
    }

    public class EnemyDamageBattleEvent : EnemyBattleEvent
    {
        public readonly int totalDamage;
        public readonly DamageModificationPackage damageModificationPackage;
        public readonly DamageSource damageSource;

        public EnemyDamageBattleEvent(Enemy enemy, int totalDamage, DamageModificationPackage damageModificationPackage, DamageSource damageSource)
            : base(BattleEventType.EnemyDamaged, enemy) =>
            (this.totalDamage, this.damageModificationPackage, this.damageSource) = (totalDamage, damageModificationPackage, damageSource);
    }
    
    public class EnemyAbilityEnemyDamageBattleEvent : EnemyDamageBattleEvent
    {
        public readonly Enemy enemyDealingDamage;

        public EnemyAbilityEnemyDamageBattleEvent(Enemy enemy, int totalDamage, DamageModificationPackage damageModificationPackage, Enemy enemyDealingDamage) : base(enemy,
            totalDamage, damageModificationPackage, DamageSource.EnemyAbility) => this.enemyDealingDamage = enemyDealingDamage;
    }

    public class EtchingEnemyDamageBattleEvent : EnemyDamageBattleEvent
    {
        public readonly Etching etching;

        public EtchingEnemyDamageBattleEvent(Enemy enemy, int totalDamage, DamageModificationPackage damageModificationPackage, Etching etching) : base(enemy, totalDamage, damageModificationPackage,
            DamageSource.Etching) => this.etching = etching;
    }
    
    public class ItemEnemyDamageBattleEvent : EnemyDamageBattleEvent
    {
        public readonly EquippedItem item;

        public ItemEnemyDamageBattleEvent(Enemy enemy, int totalDamage, DamageModificationPackage damageModificationPackage, EquippedItem item) : base(enemy, totalDamage, damageModificationPackage,
            DamageSource.Item) => this.item = item;
    }
}
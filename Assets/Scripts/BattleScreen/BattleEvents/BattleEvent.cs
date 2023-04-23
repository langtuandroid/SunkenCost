using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen.BattleEvents;
using Damage;
using Enemies;
using Etchings;
using Items.Items;

namespace BattleScreen
{
    public enum BattleEventType
    {
        None,
        StartedBattle,
        StartedEnemyTurn,
        EndedEnemyTurn,
        EndedBattle,
        StartedIndividualEnemyTurn,
        EnemyStartMove,
        EnemyMove,
        EnemyEndMyMove,
        EnemyMovementModified,
        EnemyBlocked,
        EnemySpeaking,
        EnemyHealed,
        EnemyMaxHealthModified,
        EnemyPoisoned,
        EnemyDamaged,
        EnemySpawned,
        EnemyKilled,
        EnemyReachedBoat,
        EtchingActivated,
        PlankDeactivated,
        PlankDestroyed,
        GenericItemActivation,
        TryGainedGold,
        GainedGold,
        PlayerMovedPlank,
        PlayerUsedMove,
        PlayerEndedTurn,
        PlayerLifeModified,
        PlayerLostLife,
        PlayerGainedLife,
        PlayerDied,
        PlankCreated,
        DesignModified,
    }
    
    

    public class BattleEvent
    {
        public readonly BattleEventType type;
        public readonly BattleEventResponder creator;
        public int modifier;
        public List<IBattleEventUpdatedUI> visualisers = new List<IBattleEventUpdatedUI>();
        public DamageSource damageSource;
        public DamageModificationPackage damageModificationPackage;
        public Enemy enemyAffectee;
        public Etching etching;
        public EquippedItem item;
        public Enemy enemyAffector;
        public int[] plankVisuals;

        public BattleEvent(BattleEventType type, BattleEventResponder creator = null)
        {
            this.type = type;
            this.creator = creator;
        }
        
        public static BattleEvent None => new BattleEvent(BattleEventType.None);
    }
}
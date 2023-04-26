using System;
using System.Collections;
using System.Collections.Generic;
using BattleScreen.BattleEvents;
using Damage;
using Enemies;
using Etchings;
using Items.Items;
using UnityEngine;

namespace BattleScreen
{
    public enum BattleEventType
    {
        None,
        StartedBattle,
        StartedNextTurn,
        StartedEnemyMovementPeriod,
        EndedEnemyTurn,
        EndedBattle,
        StartedIndividualEnemyTurn,
        EnemyStartMove,
        EnemyMove,
        EndedIndivdualEnemyMove,
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
        EtchingStunned,
        EtchingUnStunned,
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
        PlankDestroyed,
        PlankMoved,
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
        public List<PlankDisplay> plankDisplays = new List<PlankDisplay>();

        public BattleEvent(BattleEventType type, BattleEventResponder creator = null)
        {
            this.type = type;
            this.creator = creator;

            var name = creator is not null ? " by " + creator.name : "";
            
            Debug.Log(type + name);
        }
        
        public static BattleEvent None => new BattleEvent(BattleEventType.None);
    }
}
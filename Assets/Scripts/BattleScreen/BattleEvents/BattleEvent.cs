using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        StartedEnemyTurn,
        StartedEnemyMovementPeriod,
        EndedEnemyTurn,
        StartNextPlayerTurn,
        EndedBattle,
        SelectedNextEnemy,
        StartedIndividualEnemyTurn,
        EnemyStartOfTurnEffect,
        EnemyAboutToMove,
        EnemyMove,
        EndedIndividualEnemyTurn,
        EnemyMovementModified,
        EnemyBlocked,
        EnemySpeaking,
        EnemyHealed,
        EnemyMaxHealthModified,
        EnemyPoisoned,
        EnemyAttacked,
        EnemyDamaged,
        EnemySpawned,
        EnemyKilled,
        EnemyReachedBoat,
        EtchingActivated,
        EtchingStunned,
        EtchingUnStunned,
        ItemActivated,
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
        public int modifier;
        public DamageSource source;
        public DamageModificationPackage damageModificationPackage;
        public int affectedResponderID;
        public int affectingResponderID;
        public int[] planksToColor;

        public BattleEvent(BattleEventType type, params int[] planksToColor)
        {
            this.type = type;
            this.planksToColor = planksToColor;
        }
        
        public static BattleEvent None => new BattleEvent(BattleEventType.None);
    }
}
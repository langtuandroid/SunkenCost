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
        EtchingMoved,
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
        public readonly int[] planksToColor;
        public bool showResponse = true;
        public int modifier = 0;
        public DamageSource source;
        public DamageModificationPackage damageModificationPackage;
        public int affectedResponderID;
        public int affectingResponderID;

        public BattleEvent(BattleEventType type, params int[] planksToColor)
        {
            this.type = type;
            this.planksToColor = planksToColor;
        }
        
        public static BattleEvent None => new BattleEvent(BattleEventType.None);
    }
}
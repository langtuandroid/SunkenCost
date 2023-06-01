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
        StartedNextPlayerTurn,
        EndedBattle,
        SelectedNextEnemy,
        StartedIndividualEnemyTurn,
        EnemyEffect,
        EnemyAboutToMove,
        EnemyMoved,
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
        EnemyAttackedBoat,
        EtchingActivated,
        EtchingStunned,
        EtchingUnStunned,
        ItemActivated,
        TryGainedGold,
        GainedGold,
        PlayerMovedPlank,
        PlayerUsedMove,
        EtchingsOrderChanged,
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
        public int primaryResponderID;
        public int secondaryResponderID;

        public BattleEvent(BattleEventType type, params int[] planksToColor)
        {
            this.type = type;
            this.planksToColor = planksToColor;
        }
        
        public static BattleEvent None => new BattleEvent(BattleEventType.None);
        public Enemy Enemy => BattleEventResponseSequencer.Current.GetEnemyByResponderID(primaryResponderID);
    }
}
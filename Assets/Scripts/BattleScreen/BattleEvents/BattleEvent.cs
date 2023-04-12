using System;
using System.Collections;
using System.Collections.Generic;
using Damage;
using Enemies;
using Etchings;
using Items.Items;

namespace BattleScreen
{
    public enum BattleEventType
    {
        None,
        StartBattle,
        StartedEnemyTurn,
        EndedEnemyTurn,
        EndedBattle,
        StartedIndividualEnemyTurn,
        EnemyStartMove,
        EnemyMove,
        EnemyEndTurn,
        EnemyDebuff,
        EnemyAbility,
        EnemyHealed,
        EnemyPoisoned,
        EnemyDamaged,
        EnemyKilled,
        EnemyReachedBoat,
        PlankActivated,
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
    }

    public class BattleEvent
    {
        public readonly BattleEventType battleEventType;

        public BattleEvent(BattleEventType battleEventType) => this.battleEventType = battleEventType;
    }
}
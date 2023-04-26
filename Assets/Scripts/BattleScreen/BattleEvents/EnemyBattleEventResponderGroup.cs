using System;
using Enemies;
using UnityEngine;

namespace BattleScreen.BattleEvents
{
    public class EnemyBattleEventResponderGroup : BattleEventResponderGroup
    {
        private Enemy _currentEnemy;
        private bool _turnOrderSet = false;
        private bool _allEnemiesMoved = true;

        public override BattleEventPackage GetNextResponse(BattleEvent battleEventToRespondTo)
        {
            if (_allEnemiesMoved && 
                (battleEventToRespondTo.type == BattleEventType.StartedBattle || 
                 battleEventToRespondTo.type == BattleEventType.EndedEnemyTurn))
            {
                _allEnemiesMoved = false;
                _turnOrderSet = false;
                return EnemySpawner.Instance.SpawnNewTurn();
            }
            
            // Register / Deregister enemies
            if (battleEventToRespondTo.type == BattleEventType.EnemySpawned &&
                !HasResponder(battleEventToRespondTo.enemyAffectee))
            {
                AddResponder(battleEventToRespondTo.enemyAffectee);
            }
            else if (battleEventToRespondTo.type == BattleEventType.EnemyKilled)
            {
                RemoveResponder(battleEventToRespondTo.enemyAffectee);
            }
            
            // See if any other enemy is responding to the event
            var baseResponse = base.GetNextResponse(battleEventToRespondTo);
            if (!baseResponse.IsEmpty) return baseResponse;

            if (battleEventToRespondTo.type == BattleEventType.StartedNextTurn && !_turnOrderSet)
            {
                EnemySequencer.Current.SetNextEnemyTurnSequence();
                _turnOrderSet = true;
                return BattleEventPackage.Empty;
            }
            
            // The only times this event will come around is when an enemy has finished it's move
            if (battleEventToRespondTo.type == BattleEventType.StartedEnemyMovementPeriod)
            {
                _currentEnemy = EnemySequencer.Current.HasEnemyToMove ? EnemySequencer.Current.GetNextEnemyToMove() : null;

                if (_currentEnemy)
                {
                    _currentEnemy.CalculateActionsForTurn();
                }
                else
                {
                    _allEnemiesMoved = true;
                }
            }
            
            // Will return empty package if the enemy has finished
            if (_currentEnemy)
            {
                var enemyEventPackage =  _currentEnemy.GetNextAction();
                if (enemyEventPackage.IsEmpty || enemyEventPackage.battleEvents[0].type == BattleEventType.EndedIndivdualEnemyMove)
                    _currentEnemy = null;

                return enemyEventPackage;
            }
            
            return BattleEventPackage.Empty;
        }
    }
}
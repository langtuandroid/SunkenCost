using System.Linq;
using BattleScreen;
using BattleScreen.BattleBoard;
using BattleScreen.BattleEvents;
using Damage;

namespace Enemies
{
    public class CurrentEnemy
    {
        private readonly Enemy _enemy;
        private readonly int _responderID;
        
        private bool _hasDealtPoison;
        private bool _hasExecutedStartOfTurnAbility;
        private bool _yetToExecuteEndOfTurnAbility;

        private bool _hasToldEveryoneImAboutToMove;

        public CurrentEnemy(Enemy enemy)
        {
            _enemy = enemy;
            _responderID = enemy.ResponderID;
        }

        public BattleEventPackage GetNextAction()
        {
            if (_enemy.IsDestroyed) return new BattleEventPackage(CreateEvent(BattleEventType.EndedIndividualEnemyTurn));
            
            if (!_hasDealtPoison)
            {
                _hasDealtPoison = true;

                if (_enemy.stats.Poison > 0)
                    return new BattleEventPackage(_enemy.DealPoisonDamage())
                        .WithIdentifier(BattleEventType.EnemyStartOfTurnEffect, _responderID);
            }
            
            if (!_hasExecutedStartOfTurnAbility)
            {
                _hasExecutedStartOfTurnAbility = true;

                if (_enemy is IStartOfTurnAbilityHolder startOfTurnAbilityHolder 
                    && startOfTurnAbilityHolder.GetIfUsingStartOfTurnAbility())
                {
                    // TODO: Battle event packages should be mergeable
                    var abilityEvents = (startOfTurnAbilityHolder.GetStartOfTurnAbility()).battleEvents.ToList();
                    return new BattleEventPackage(abilityEvents)
                        .WithIdentifier(BattleEventType.EnemyStartOfTurnEffect, _responderID);
                }
            }

            if (!_enemy.Mover.FinishedMoving)
            {
                // Tell everyone I'm about to move before each move
                if (!_hasToldEveryoneImAboutToMove)
                {
                    _hasToldEveryoneImAboutToMove = true;
                    return new BattleEventPackage(CreateEvent(BattleEventType.EnemyAboutToMove));
                }

                _hasToldEveryoneImAboutToMove = false;
                
                // Change my plank
                _enemy.Mover.Move();
                
                // I've either moved a plank or reached the boat and died
                return (_enemy.PlankNum >= Board.Current.PlankCount)
                    ? _enemy.Die(DamageSource.Boat)
                    : new BattleEventPackage(CreateEvent(BattleEventType.EnemyMove));
            }
            
            //TODO: Add EndOfTurnAbility processing here
            
            
            return new BattleEventPackage(CreateEvent(BattleEventType.EndedIndividualEnemyTurn));
        }

        private BattleEvent CreateEvent(BattleEventType battleEventType)
        {
            return new BattleEvent(battleEventType) { affectedResponderID = _responderID };
        }
    }
}
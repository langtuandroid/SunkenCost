using System;
using System.Collections.Generic;
using System.Linq;
using Damage;
using Designs;
using Enemies;
using Etchings;
using UnityEngine;

namespace BattleScreen.BattleEvents
{
    public class BattleEventsManager : MonoBehaviour
    {
        public static BattleEventsManager Current;
        
        private EnemySequencer _enemySequencer;
        
        [SerializeField] private BattleEventResponderGroup _itemManager;
        [SerializeField] private BattleEventResponderGroup _enemiesManager;
        [SerializeField] private BattleEventResponderGroup _etchingManager;
        [SerializeField] private BattleEventResponderGroup _playerManager;
        
        private BattleEventResponderGroup[] _responderGroupOrder;
        
        private readonly BattleEventResponderTracker _battleEventResponderTracker = new BattleEventResponderTracker();

        private bool _hasCurrentEnemy;
        private CurrentEnemy _currentEnemy;

        private void Awake()
        {
            if (Current)
            {
                Destroy(Current.gameObject);
            }

            Current = this;

            _responderGroupOrder = new []{_playerManager, _enemiesManager, _etchingManager, _itemManager};
        }

        private void Start()
        {
            _enemySequencer = EnemySequencer.Current;
        }

        public BattleEventPackage GetNextResponse(BattleEvent battleEvent)
        {
            var index = _battleEventResponderTracker.GetOrCreateIndex(battleEvent);

            // Check each of the responder groups
            while (index < _responderGroupOrder.Length)
            {
                var responsePackage = _responderGroupOrder[index].GetNextResponse(battleEvent);
                if (!responsePackage.IsEmpty) return responsePackage;

                index++;
                _battleEventResponderTracker.SetIndex(battleEvent, index);
            }

            if (battleEvent.type != BattleEventType.StartedEnemyMovementPeriod) return BattleEventPackage.Empty;

            // Select next enemy
            if (!_hasCurrentEnemy)
            {
                // Out of enemies
                if (!_enemySequencer.HasEnemyToMove) return BattleEventPackage.Empty;

                _hasCurrentEnemy = true;
                var enemy = _enemySequencer.SelectNextEnemy();
                _currentEnemy = new CurrentEnemy(enemy);
                return new BattleEventPackage(new BattleEvent(BattleEventType.StartedIndividualEnemyTurn) 
                    {primaryResponderID = enemy.ResponderID});
            }
            
            var enemyResponse = _currentEnemy.GetNextAction();
            Debug.Log("Current enemy executing actions: " + 
                      String.Join(", ", 
                          enemyResponse.battleEvents.ConvertAll(i => i.type.ToString()).ToArray()));
                    
            if (enemyResponse.battleEvents[0].type == BattleEventType.EndedIndividualEnemyTurn)
            {
                _hasCurrentEnemy = false;
            }

            return enemyResponse;
        }
        
        public DamageModificationPackage GetDamageModifiers(EnemyDamage damage)
        {
            var modifiers = _responderGroupOrder.Select
                (g => g.GetDamageModifiers(damage)).ToList();
            
            var flatTotal = modifiers.SelectMany(package => package.flatModifications).ToList();
            var multiTotal = modifiers.SelectMany(package => package.multiModifications).ToList();
            
            return new DamageModificationPackage(flatTotal, multiTotal);
        }

        public Enemy GetEnemyByResponderID(int id)
        {
            var responder = BattleEventResponder.AllBattleEventRespondersByID[id];
            if (responder is Enemy enemy)
            {
                return enemy;
            }
            
            throw new Exception("Battle responder is not enemy! Instead it's " + responder.GetType().Name);
        }
        
        public Etching GetEtchingByResponderID(int id)
        {
            var responder = BattleEventResponder.AllBattleEventRespondersByID[id];
            if (responder is Etching etching)
            {
                return etching;
            }
            
            throw new Exception("Battle responder is not etching!");
        }
    }
}
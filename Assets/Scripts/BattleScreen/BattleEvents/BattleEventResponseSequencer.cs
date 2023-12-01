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
    public class BattleEventResponseSequencer : MonoBehaviour
    {
        public static BattleEventResponseSequencer Current;
        
        private EnemySequencer _enemySequencer;
        
        [SerializeField] private BattleEventResponderGroup _itemManager;
        [SerializeField] private BattleEventResponderGroup _enemiesManager;
        [SerializeField] private BattleEventResponderGroup _etchingManager;
        [SerializeField] private BattleEventResponderGroup _playerManager;
        
        private BattleEventResponderGroup[] _responderGroupOrder;
        
        private readonly BattleEventResponseIterationTracker _battleEventResponseIterationTracker = new BattleEventResponseIterationTracker();

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
            var index = _battleEventResponseIterationTracker.GetOrCreateIndex(battleEvent);

            // Check each of the responder groups
            while (index < _responderGroupOrder.Length)
            {
                var responsePackage = _responderGroupOrder[index].GetNextResponse(battleEvent);
                if (!responsePackage.IsEmpty) return responsePackage;

                index++;
                _battleEventResponseIterationTracker.SetIndex(battleEvent, index);
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
                          enemyResponse.battleEvents.ConvertAll(i => i.type + "(" + i.modifier + ")").ToArray()));
                    
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

        public void RefreshTransforms()
        {
            foreach (var battleEventResponderGroup in _responderGroupOrder)
            {
                battleEventResponderGroup.RefreshTransforms();
            }
        }
        
        public Enemy GetEnemyByResponderID(int id)
        {
            return GetTypeByResponderID<Enemy>(id);
        }
        
        public Etching GetEtchingByResponderID(int id)
        {
            return GetTypeByResponderID<Etching>(id);
        }
        
        private T GetTypeByResponderID<T>(int id) where T : BattleEventResponder
        {
            var responder = BattleEventResponder.AllBattleEventRespondersByID[id];
            if (responder is T t)
            {
                return t;
            }
            
            throw new Exception("Battle responder is not " + typeof(T).Name + "! Instead it's " + responder.GetType().Name);
        }
    }
}
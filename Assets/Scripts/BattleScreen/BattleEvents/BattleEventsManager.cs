using System.Collections.Generic;
using System.Linq;
using Damage;
using Designs;
using UnityEngine;

namespace BattleScreen.BattleEvents
{
    public readonly struct BattleEventPackage
    {
        public readonly BattleEvent[] battleEvents;

        public static BattleEventPackage Empty => new BattleEventPackage(BattleEvent.None);
        public bool IsEmpty => battleEvents[0].type == BattleEventType.None;

        public BattleEventPackage(params BattleEvent[] battleEvents)
        {
            this.battleEvents = battleEvents;
        }

        public BattleEventPackage(IEnumerable<BattleEvent> battleEventsList) : this(battleEventsList.ToArray())
        {
        }
    }
    
    public class BattleEventsManager : MonoBehaviour
    {
        public static BattleEventsManager Current;
        
        [SerializeField] private BattleEventResponderGroup _itemManager;
        [SerializeField] private BattleEventResponderGroup _enemiesManager;
        [SerializeField] private BattleEventResponderGroup _etchingManager;
        [SerializeField] private BattleEventResponder _player;
        [SerializeField] private EnemySequencer _enemySequencer;
        
        private BattleEventResponderGroup[] _responderGroupOrder;
        
        private readonly BattleEventResponderTracker _battleEventResponderTracker = new BattleEventResponderTracker();

        private void Awake()
        {
            if (Current)
            {
                Destroy(Current.gameObject);
            }

            Current = this;

            _responderGroupOrder = new []{_enemiesManager, _etchingManager, _itemManager};
        }
        
        public BattleEventPackage GetNextResponse(BattleEvent battleEvent)
        {
            var index = _battleEventResponderTracker.GetIndex(battleEvent);

            while (index < _responderGroupOrder.Length)
            {
                var responsePackage = _responderGroupOrder[index].GetNextResponse(battleEvent);
                if (!responsePackage.IsEmpty) return responsePackage;

                index++;
                _battleEventResponderTracker.SetIndex(battleEvent, index);
            }

            return BattleEventPackage.Empty;
        }
        
        public DamageModificationPackage GetDamageModifiers(BattleEvent battleEvent)
        {
            var modifiers = _responderGroupOrder.Select
                (g => g.GetDamageModifiers(battleEvent)).ToList();
            
            var flatTotal = modifiers.SelectMany(package => package.flatModifications).ToList();
            var multiTotal = modifiers.SelectMany(package => package.multiModifications).ToList();
            
            return new DamageModificationPackage(flatTotal, multiTotal);
        }

        /*
        public List<BattleEvent> StartBattle()
        {
            var startBattleEvents = GetEventAndResponsesList(new BattleEvent(BattleEventType.StartedBattle, null));

            EnemySpawner.Instance.StartBattle();
            startBattleEvents.AddRange(SpawnEnemies());
            return startBattleEvents;
        }

        public List<BattleEvent> GetNextTurn()
        {
            var turnEvents = new List<BattleEvent>();
            
            // Start of turn processing
            var startTurnBattleEvents = GetEventAndResponsesList(new BattleEvent(BattleEventType.StartedEnemyTurn, null));

            // Iterate through the enemies' moves
            var movementBattleEvents = _enemyController.GetMovements();
            
            // End of turn
            var endTurnBattleEvents = GetEventAndResponsesList(new BattleEvent(BattleEventType.EndedEnemyTurn, null));

            turnEvents.AddRange(startTurnBattleEvents);
            turnEvents.AddRange(movementBattleEvents);
            turnEvents.AddRange(endTurnBattleEvents);
            turnEvents.AddRange(SpawnEnemies());

            return turnEvents;
        }

        public List<BattleEvent> EndBattle()
        {
            return GetEventAndResponsesList(new BattleEvent(BattleEventType.EndedBattle, null));
        }

        private List<BattleEvent> GetEventAndResponsesList(BattleEvent battleEvent)
        {
            foreach (var ui in _eventRespondingUI.Where(ui => ui.GetIfUpdating(battleEvent)))
            {
                Debug.Log(ui + " saving state!");
                ui.SaveStateResponse(battleEvent.type);
                battleEvent.visualisers.Add(ui);
            }
            
            var responses = new List<BattleEvent> {battleEvent};

            foreach (var responderGroup in _responderOrder)
            {
                var groupResponses = GetResponsesFromGroup(responderGroup, battleEvent);
                responses.AddRange(groupResponses);
            }
            
            if (_player.GetIfRespondingToBattleEvent(battleEvent))
                responses.AddRange(_player.GetResponseToBattleEvent(battleEvent));

            return responses;
        }

        

        private List<BattleEvent> GetResponsesFromGroup(BattleEventResponderGroup group,
            BattleEvent battleEvent)
        {
            var groupResponse = new List<BattleEvent>();

            if (battleEvent.type == BattleEventType.PlayerDied || battleEvent.type == BattleEventType.EndedBattle)
                return groupResponse;
            
            var responders = group.GetEventResponders(battleEvent);

            foreach (var turnActionResponder in responders)
            {
                groupResponse.AddRange(turnActionResponder.GetResponseToBattleEvent(battleEvent));
            }

            return groupResponse;
        }


        private List<BattleEvent> SpawnEnemies()
        {
            var battleEvents = new List<BattleEvent>();
            foreach (var enemySpawnEvent in EnemySpawner.Instance.SpawnNewTurn())
            {
                battleEvents.AddRange(GetEventAndResponsesList(enemySpawnEvent));
            }

            return battleEvents;
        }
        
        */
    }
}
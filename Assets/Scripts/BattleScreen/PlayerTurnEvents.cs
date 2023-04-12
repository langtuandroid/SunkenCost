using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleScreen.BattleEvents;
using UnityEngine;

namespace BattleScreen
{
    public class PlayerTurnEvents : MonoBehaviour
    {
        public static PlayerTurnEvents Current;

        private Queue<BattleEvent> _playerEventsQueue = new Queue<BattleEvent>();

        public bool Executing { get; private set; } = false;

        private void Awake()
        {
            if (Current)
                Destroy(Current.gameObject);

            Current = this;
        }

        public void InvokeEvent(BattleEvent battleEvent)
        {
            var additionalList = BattleEventsManager.Current.GetEventAndResponsesList(battleEvent);
            _playerEventsQueue = new Queue<BattleEvent>(_playerEventsQueue.Concat(additionalList));

            if (Executing) return;
            
            StartCoroutine(PlayOutEventQueue());
        }

        private IEnumerator PlayOutEventQueue()
        {
            Executing = true;
            yield return Battle.VisualiseBattleEvents(_playerEventsQueue);
            Executing = false;
        }
    }
}
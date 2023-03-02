using System.Collections.Generic;
using System.Linq;
using Challenges.Listeners;
using UnityEngine;

namespace Challenges
{
    public class InBattleChallengeListener : MonoBehaviour
    {
        private List<Challenge> _activeChallenges;
        private IEnumerable<IKillListener> _killListeners;
        private IEnumerable<IEndOfBattleListener> _endOfBattleListeners;
        
        private void Start()
        {
            BattleEvents.Current.OnEnemyAttacked += EnemyAttacked;
            BattleEvents.Current.OnEndBattle += EndedBattle;

            _activeChallenges = RunProgress.activeChallenges;
            _killListeners = _activeChallenges.OfType<IKillListener>();
            _endOfBattleListeners = _activeChallenges.OfType<IEndOfBattleListener>();
            
        }

        private void EnemyAttacked()
        {
            if (!BattleEvents.LastEnemyAttacked.IsDestroyed) return;
            
            foreach (var listener in _killListeners)
            {
                listener.EnemyKilled();
            }
        }

        private void EndedBattle()
        {
            foreach (var listener in _endOfBattleListeners)
            {
                listener.EndOfBattle();
            }
        }
    }
}
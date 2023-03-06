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
        private IEnumerable<IPlayerLostLifeListener> _playerLostLifeListeners;
        
        private void Start()
        {
            BattleEvents.Current.OnEnemyAttacked += EnemyAttacked;
            BattleEvents.Current.OnEndBattle += EndedBattle;
            BattleEvents.Current.OnLostLife += LostLife;

            _activeChallenges = RunProgress.ActiveChallenges;
            _killListeners = _activeChallenges.OfType<IKillListener>();
            _endOfBattleListeners = _activeChallenges.OfType<IEndOfBattleListener>();
            _playerLostLifeListeners = _activeChallenges.OfType<IPlayerLostLifeListener>();

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

        private void LostLife()
        {
            foreach (var listener in _playerLostLifeListeners)
            {
                listener.PlayerLostLife();
            }
        }
    }
}
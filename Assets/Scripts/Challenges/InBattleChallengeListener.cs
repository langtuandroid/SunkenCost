using System;
using System.Collections.Generic;
using System.Linq;
using EventListeners;
using UnityEngine;

namespace Challenges
{
    public class InBattleChallengeListener : MonoBehaviour
    {
        private List<Challenge> activeChallenges;
        private void Start()
        {
            activeChallenges = RunProgress.ActiveChallenges;
            
            foreach (var listener in activeChallenges.OfType<IStartOfBattleListener>())
            {
                BattleEvents.Current.OnStartBattle += listener.StartOfBattle;
            }

            foreach (var listener in activeChallenges.OfType<IEndOfBattleListener>())
            {
                BattleEvents.Current.OnEndBattle += listener.EndOfBattle;
            }
            
            foreach (var listener in activeChallenges.OfType<IKillListener>())
            {
                BattleEvents.Current.OnEnemyKilled += listener.EnemyKilled;
            }
            
            foreach (var listener in activeChallenges.OfType<IPlayerLostLifeListener>())
            {
                BattleEvents.Current.OnPlayerLostLife += listener.PlayerPlayerLostLife;
            }
        }
    }
}
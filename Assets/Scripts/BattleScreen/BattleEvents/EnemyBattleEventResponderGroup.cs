using System;
using Enemies;
using UnityEngine;

namespace BattleScreen.BattleEvents
{
    public class EnemyBattleEventResponderGroup : BattleEventResponderGroup
    {
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private EnemySequencer _enemySequencer;

        private Enemy _currentEnemy;

        private void Awake()
        {
            AddResponder(_enemySpawner);
            AddResponder(_enemySequencer);
        }

        public override BattleEventPackage GetNextResponse(BattleEvent battleEventToRespondTo)
        {
            switch (battleEventToRespondTo.type)
            {
                // Register / Deregister enemies
                case BattleEventType.EnemySpawned when !HasResponder(battleEventToRespondTo.enemyAffectee):
                    AddResponder(battleEventToRespondTo.enemyAffectee);
                    break;
                case BattleEventType.EnemyKilled:
                    RemoveResponder(battleEventToRespondTo.enemyAffectee);
                    break;
            }
            
            return base.GetNextResponse(battleEventToRespondTo);
        }
    }
}
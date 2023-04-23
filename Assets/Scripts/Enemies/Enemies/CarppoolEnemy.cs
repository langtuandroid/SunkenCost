using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using UnityEngine;

namespace Enemies.Enemies
{
    public class CarppoolEnemy : Enemy, IStartOfTurnAbilityHolder
    {
        protected override void Init()
        {
            Size = 1.2f;
            Name = "Carppool";
            Mover.AddMove(1);
            SetInitialHealth(50);
            Gold = 10;
        }
        
        public override string GetDescription()
        {
            return "Spawns a Bull Carp every turn";
        }

        public bool GetIfUsingStartOfTurnAbility()
        {
            return true;
        }

        public List<BattleEvent> GetStartOfTurnAbility()
        {
            var newBullCarp = EnemySpawner.Instance.SpawnEnemyOnPlank("BullCarp", PlankNum);
            var newEvent = CreateEvent(BattleEventType.EnemySpawned);
            newEvent.enemyAffectee = newBullCarp;
            newEvent.enemyAffector = this;
            return new List<BattleEvent>(){newEvent};
        }
    }
}
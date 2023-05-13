using System.Collections;
using System.Collections.Generic;
using BattleScreen;
using BattleScreen.BattleEvents;
using UnityEngine;

namespace Enemies.Enemies
{
    public class CarppoolEnemy : EliteEnemy, IStartOfTurnAbilityHolder
    {
        public override string GetDescription()
        {
            return "Spawns a Bull Carp every turn";
        }

        public bool GetIfUsingStartOfTurnAbility()
        {
            return true;
        }

        public BattleEventPackage GetStartOfTurnAbility()
        {
            var newBullCarp = EnemySpawner.Instance.SpawnEnemyDuringTurn(EnemyType.BullCarp, PlankNum);
            var newEvent = CreateEvent(BattleEventType.EnemySpawned);
            newEvent.primaryResponderID = newBullCarp.ResponderID;
            newEvent.secondaryResponderID = ResponderID;
            return new BattleEventPackage(newEvent);
        }
    }
}
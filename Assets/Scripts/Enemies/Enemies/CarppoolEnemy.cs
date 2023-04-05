using System.Collections;
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

        public IEnumerator ExecuteStartOfTurnAbility()
        {
            EnemySpawner.current.SpawnActiveEnemy("BullCarp", StickNum);
            yield break;
        }
    }
}
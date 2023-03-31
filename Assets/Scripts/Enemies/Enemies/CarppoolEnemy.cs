using UnityEngine;

namespace Enemies.Enemies
{
    public class CarppoolEnemy : Enemy
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
            return "Spawns two Bull Carp every turn";
        }

        protected override bool TestForStartOfTurnAbility()
        {
            return true;
        }

        protected override void StartOfTurnAbility()
        {
            EnemySpawner.current.SpawnEnemy("BullCarp", StickNum);
            EnemySpawner.current.SpawnEnemy("BullCarp", StickNum);
        }
    }
}
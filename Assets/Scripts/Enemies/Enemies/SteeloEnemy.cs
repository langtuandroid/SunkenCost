using UnityEngine;

namespace Enemies.Enemies
{
    public class SteeloEnemy : Enemy
    {
        protected override void Init()
        {
            Size = 1.2f;
            Name = "Steelo";
            Mover.AddMove(1);
            SetInitialHealth(100);
            Gold = 2;
        }

        public override string GetDescription()
        { 
            return "Big, slow, harder snail";
        }
    }
}
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
            SetInitialHealth(50);
            Gold = 5;
        }

        public override string GetDescription()
        { 
            return "Big, slow, harder snail";
        }
    }
}
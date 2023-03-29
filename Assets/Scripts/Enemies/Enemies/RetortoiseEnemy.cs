namespace Enemies.Enemies
{
    public class RetortoiseEnemy : Enemy
    {
        protected override void Init()
        {
            Size = 1.2f;
            Name = "Retortoise";
            Mover.AddMove(0);
            SetInitialHealth(70);
            Gold = 5;
        }

        public override string GetDescription()
        { 
            return "Gets faster each time it's attacked";
        }

        public override void TakeDamage(int damage, DamageSource damageSource)
        {
            if (damageSource != DamageSource.Plank) return;
            
            Mover.AddMovementModifier(1);

            base.TakeDamage(damage, damageSource);
        }
    }
}
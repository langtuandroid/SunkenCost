namespace Enemies.Enemies
{
    public class RetortoiseEnemy : Enemy
    {
        protected override void Init()
        {
            Size = 1.2f;
            Name = "Retortoise";
            Mover.AddMove(1);
            SetInitialHealth(50);
            Gold = 5;
        }

        public override string GetDescription()
        { 
            return "Gets faster each time it's attacked";
        }

        public override void TakeDamage(int damage, DamageSource damageSource)
        {
            if (damageSource != DamageSource.Etching) return;
            
            Mover.AddMovementModifier(1);

            base.TakeDamage(damage, damageSource);
        }
    }
}
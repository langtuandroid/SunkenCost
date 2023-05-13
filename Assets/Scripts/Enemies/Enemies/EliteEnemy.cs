namespace Enemies.Enemies
{
    public abstract class EliteEnemy : Enemy
    {
        protected override void Awake()
        {
            Gold = 5;
            Size = 1.2f;
            base.Awake();
        }
    }
}
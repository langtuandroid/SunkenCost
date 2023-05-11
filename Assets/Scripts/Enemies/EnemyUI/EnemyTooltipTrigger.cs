namespace Enemies.EnemyUI
{
    public class EnemyTooltipTrigger : TooltipTrigger
    {
        private Enemy _enemy;

        private void Start()
        {
            _enemy = transform.parent.GetComponent<Enemy>();
        }
    
        protected override string GetContent()
        {
            return _enemy.GetDescription();
        }
    }
}

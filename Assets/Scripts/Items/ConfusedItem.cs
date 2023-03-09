namespace Items
{
    public class ConfusedItem : InBattleItem
    {
        protected override void Activate()
        {
            StickManager.current.RandomisePlanks();
        }
    }
}
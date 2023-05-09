namespace Items
{
    public class ItemOffer
    {
        public readonly ItemAsset itemAsset;
        public readonly ItemInstance itemInstance;
        public int Cost { get; private set; }

        public ItemOffer(ItemAsset itemAsset)
        {
            this.itemAsset = itemAsset;
            itemInstance = new ItemInstance(itemAsset, itemAsset.modifier);
            
            Cost = RunProgress.PlayerStats.PriceHandler.GetItemCost(itemInstance.Rarity);
        }

        public void SetCost(int cost)
        {
            Cost = cost;
        }
    }
}
namespace Items
{
    public class ItemOffer
    {
        public readonly ItemAsset itemAsset;
        public int Cost { get; private set; }
        
        public ItemOffer(ItemAsset itemAsset)
        {
            this.itemAsset = itemAsset;
            Cost = RunProgress.PlayerStats.PriceHandler.GetItemCost(itemAsset.rarity);
        }
    }
}
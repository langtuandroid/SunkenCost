namespace Items
{
    public class ItemOffer
    {
        public string ItemId;
        public string Title { get; set; }
        public string Desc { get; set; }
        public int Rarity { get; set; }
        
        public int Cost { get; set; }

        public ItemOffer(string itemId, string title, string desc, int rarity)
        {
            ItemId = itemId;
            Title = title;
            Desc = desc;
            Rarity = rarity;

            Cost = RunProgress.PlayerProgress.PriceHandler.GetItemCost(rarity);
        }
    }
}
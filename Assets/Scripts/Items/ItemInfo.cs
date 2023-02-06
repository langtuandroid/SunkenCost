namespace Items
{
    public class ItemInfo
    {
        public string ItemId;
        public string Title { get; set; }
        public string Desc { get; set; }
        public int Cost { get; set; }

        public ItemInfo(string itemId, string title, string desc, int cost)
        {
            ItemId = itemId;
            Title = title;
            Desc = desc;
            Cost = cost;
        }
    }
}
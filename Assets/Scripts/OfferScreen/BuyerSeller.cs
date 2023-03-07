namespace OfferScreen
{
    public class BuyerSeller
    {
        public int Gold { get; private set; } = 0;
        private readonly GoldDisplay _goldDisplay;
        
        public BuyerSeller(int gold, GoldDisplay goldDisplay)
        {
            _goldDisplay = goldDisplay;
            AlterGold(gold);
        }

        public void Buy(int cost)
        {
            AlterGold(-cost);
        }

        public void Sell(int cost)
        {
            AlterGold(cost);
        }

        private void AlterGold(int amount)
        {
            Gold += amount;
            _goldDisplay.UpdateText(Gold);
        }
    }
}
using UnityEngine;

namespace OfferScreen
{
    public class PriceHandler
    {
        private const int BASE_CARD_COST_VARIATION = 1;
        private const int BASE_ITEM_COST_VARIATION = 4;
        private const int BASE_CARD_COST_MULTIPLIER = 3;
        private const int BASE_ITEM_COST_MULTIPLIER = 10;

        private readonly Stat _cardCostMultiplier;
        private readonly Stat _itemCostMultiplier;

        public PriceHandler()
        {
            _cardCostMultiplier = new Stat(BASE_CARD_COST_MULTIPLIER);
            _itemCostMultiplier = new Stat(BASE_ITEM_COST_MULTIPLIER);
        }

        public int GetCardCost(int cardRarity)
        {
            return cardRarity * _cardCostMultiplier.Value
                   + Random.Range(-BASE_CARD_COST_VARIATION, BASE_CARD_COST_VARIATION);
        }

        public int GetItemCost(int itemRarity)
        {
            return itemRarity * _itemCostMultiplier.Value
                   + Random.Range(-BASE_ITEM_COST_VARIATION, BASE_ITEM_COST_VARIATION);;
        }
    }
}
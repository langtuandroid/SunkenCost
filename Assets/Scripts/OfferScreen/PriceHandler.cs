using System;
using Items;
using Pickups;
using Random = UnityEngine.Random;

namespace OfferScreen
{
    public class PriceHandler
    {
        private const int BASE_CARD_COST_VARIATION = 2;
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

        public int GetCardCost(Rarity rarity)
        {
            return (int)rarity * _cardCostMultiplier.Value
                   + Random.Range(-BASE_CARD_COST_VARIATION, BASE_CARD_COST_VARIATION);
        }

        public int GetItemCost(Rarity rarity)
        {
            var cost = Random.Range(-BASE_ITEM_COST_VARIATION, BASE_ITEM_COST_VARIATION);
            switch (rarity)
            {
                case Rarity.Common:
                    cost += 10;
                    break;
                case Rarity.Uncommon:
                    cost += 17;
                    break;
                case Rarity.Rare:
                    cost += 25;
                    break;
                case Rarity.ElitePickup:
                default:
                    throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null);
            }

            return cost;
        }
    }
}
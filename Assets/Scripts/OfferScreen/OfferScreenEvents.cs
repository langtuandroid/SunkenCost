using System;
using UnityEngine;

namespace OfferScreen
{
    public class OfferScreenEvents : MonoBehaviour
    {
        [SerializeField] private PlankCount plankCount;
    
        public static OfferScreenEvents Current;

        public event Action OnOffersRefreshed;

        private void Awake()
        {
            if (Current)
            {
                Destroy(Current.gameObject);
            }

            Current = this;
        }

        public void RefreshOffers()
        {
            OnOffersRefreshed?.Invoke();
            plankCount.UpdateText(OfferManager.Current.CardsOnTopRow);
        }
    }
}

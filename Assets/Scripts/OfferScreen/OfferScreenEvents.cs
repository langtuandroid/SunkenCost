using System;
using UnityEngine;

namespace OfferScreen
{
    public class OfferScreenEvents : MonoBehaviour
    {
        [SerializeField] private PlankCount plankCount;
    
        public static OfferScreenEvents Current;

        public event Action OnGridsUpdated;

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
            OnGridsUpdated?.Invoke();
            plankCount.UpdateText(OfferManager.CardsOnTopRow);
        }
    }
}

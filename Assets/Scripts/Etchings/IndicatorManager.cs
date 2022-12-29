using UnityEngine;

namespace Etchings
{
    public class IndicatorManager : MonoBehaviour
    {
        private void Start()
        {
            BattleEvents.Current.OnSticksUpdated += SticksUpdated;
        }

        private void SticksUpdated()
        {
            foreach (var etching in EtchingManager.Current.etchingOrder)
            {
                etching.UpdateIndicators();
            }
        }
    }
}

using UnityEngine;

namespace Etchings
{
    public class IndicatorManager : MonoBehaviour
    {
        private void Start()
        {
            OldBattleEvents.Current.OnSticksUpdated += SticksUpdated;
        }

        private void SticksUpdated()
        {
            foreach (var etching in EtchingMap.Current.etchingOrder)
            {
                etching.UpdateIndicators();
            }
        }
    }
}

using UnityEngine.EventSystems;

namespace OfferScreen
{
    public interface IOffer : IPointerEnterHandler ,IPointerExitHandler
    {
        public void ClickedLockButton();
    }
}
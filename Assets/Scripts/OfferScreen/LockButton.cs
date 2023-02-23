using System;
using UnityEngine;

namespace OfferScreen
{
    public class LockButton : MonoBehaviour
    {

        private CanvasGroup _canvasGroup;
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            Hide();
        }

        public void Show()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.alpha = 1;
        }

        public void Hide()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.alpha = 0;
        }
    }
}
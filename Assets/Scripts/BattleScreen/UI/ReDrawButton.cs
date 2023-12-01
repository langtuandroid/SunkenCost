using System;
using BattleScreen.BattleBoard;
using BattleScreen.BattleEvents;
using OfferScreen;
using UnityEngine;
using UnityEngine.UI;

namespace BattleScreen.UI
{
    public class ReDrawButton : MonoBehaviour, IBattleEventUpdatedUI
    {
        [SerializeField] private Plank _plank;
        [SerializeField] private CostDisplay _costDisplay;
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;

        private Action _listener;
        
        private int Cost => RunProgress.Current.PlayerStats.ReRollCost;
        private bool CanBuy => Cost <= RunProgress.Current.PlayerStats.Gold;

        private void Awake()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void Start()
        {
            BattleRenderer.Current.RegisterUIUpdater(this);
            _costDisplay.Refresh(Cost, CanBuy);
        }
        
        private void OnDestroy()
        {
            if (BattleRenderer.Current)
                BattleRenderer.Current.DeregisterUIUpdater(this);
        }

        public void RespondToBattleEvent(BattleEvent battleEvent)
        {
            if (battleEvent.type == BattleEventType.AlteredGold)
                _costDisplay.Refresh(Cost, CanBuy);
        }

        public void Hide()
        {
            _costDisplay.gameObject.SetActive(false);
            _image.enabled = false;
            _button.enabled = false;
        }

        public void Show()
        {
            _costDisplay.gameObject.SetActive(true);
            _image.enabled = true;
            _button.enabled = true;
        }

        private void OnClick()
        {
            if (CanBuy)
            {
                Battle.Current.InvokeResponsesToPlayerTurnEvent(new BattleEvent(BattleEventType.TriedAlterGold)
                {
                    modifier = -RunProgress.Current.PlayerStats.ReRollCost
                });
                Battle.Current.InvokeResponsesToPlayerTurnEvent(new BattleEvent(BattleEventType.PlayerRolled)
                {
                    primaryResponderID = _plank.Etching.ResponderID
                });
            }
        }
    }
}
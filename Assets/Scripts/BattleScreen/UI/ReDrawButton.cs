using System;
using BattleScreen.BattleEvents;
using OfferScreen;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BattleScreen.UI
{
    public class ReDrawButton : MonoBehaviour, IBattleEventUpdatedUI
    {
        [SerializeField] private CostDisplay _costDisplay;
        [SerializeField] private Button _button;

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

            if (battleEvent.type == BattleEventType.StartedEnemyTurn)
            {
                Destroy(gameObject);
            }
        }

        public void SetListener(Action action)
        {
            _listener = action;
        }

        private void OnClick()
        {
            if (CanBuy)
            {
                _listener.Invoke();
                
                RunProgress.Current.PlayerStats.Gold -= RunProgress.Current.PlayerStats.ReRollCost;
                Battle.Current.InvokeResponsesToPlayerTurnEvent(new BattleEvent(BattleEventType.ReDrewPlanks));
            }
        }
    }
}
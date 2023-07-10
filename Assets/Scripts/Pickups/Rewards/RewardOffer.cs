using BattleScreen;
using UI.Tooltip;
using UnityEngine;
using UnityEngine.UI;

namespace Pickups.Rewards
{
    public class RewardOffer : MonoBehaviour, IDynamicTooltipTriggerListener
    {
        [SerializeField] private Image _image;
        [SerializeField] private Image _cardBackground;

        public Reward Reward { get; private set; }

        public void Init(Reward reward)
        {
            Reward = reward;
            _image.sprite = reward.GetSprite();

            if (reward.RewardType == RewardType.Card || reward.RewardType == RewardType.EliteCard)
                _cardBackground.enabled = true;
        }

        public void Click()
        {
            RunProgress.Current.AcceptReward(Reward);
            Battle.Current.LeaveBattle();
        }
        
        public string GetTitle() => Reward.GetTitle();
        public string GetDescription() => Reward.GetDescription();
    }
}
using System;
using BattleScreen;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BattleSpeedButton : InGameButton
    {
        [SerializeField] private Sprite _playSprite;
        [SerializeField] private Sprite _fastForwardSprite;
        [SerializeField] private Sprite _ultraSprite;

        private BattleSpeed _currentSpeed = Settings.BattleSpeed;

        private void Start()
        {
            SetBattleSpeed(_currentSpeed);
        }

        protected override bool TryClick()
        {
            _currentSpeed = _currentSpeed switch
            {
                BattleSpeed.Normal => BattleSpeed.Fast,
                BattleSpeed.Fast => BattleSpeed.Ultra,
                BattleSpeed.Ultra => BattleSpeed.Normal,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            SetBattleSpeed(_currentSpeed);
            return true;
        }
        
        

        private void SetBattleSpeed(BattleSpeed newBattleSpeed)
        {
            Battle.Current.SetBattleSpeed(_currentSpeed);
            SetSprite(newBattleSpeed);
        }

        private void SetSprite(BattleSpeed newBattleSpeed)
        {
            Image.sprite = _currentSpeed switch
            {
                BattleSpeed.Normal => _playSprite,
                BattleSpeed.Fast => _fastForwardSprite,
                BattleSpeed.Ultra => _ultraSprite,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
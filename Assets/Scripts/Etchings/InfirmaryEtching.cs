using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Etchings
{
    public class InfirmaryEtching : ActiveEtching
    {
        private Stat _healAmountStat;
        
        protected override void Start()
        {
            _healAmountStat = new Stat(design.GetStat(St.HealPlayer));
            Debug.Log(_healAmountStat.Value);
            colorWhenActivated = true;

            BattleEvents.Current.OnEndBattle += HealPlayer;
            base.Start();
        }

        protected override bool CheckInfluence(int stickNum)
        {
            return stickNum == Stick.GetStickNumber();
        }

        private void HealPlayer()
        {
            PlayerController.current.AddLife(_healAmountStat.Value);
        }
    }
}
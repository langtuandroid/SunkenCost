using System.Collections.Generic;
using Designs;
using Enemies;
using UnityEngine;

namespace Etchings
{
    public class RestEtching : ActiveEtching
    {
        private Stat _healAmountStat;
        
        protected override void Start()
        {
            _healAmountStat = new Stat(design.GetStat(StatType.HealPlayer));
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
            Stick.SetTempColour(design.Color);
            StartCoroutine(ColorForActivate());
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Designs;
using UnityEngine;

namespace Etchings
{
    public class BoostEtching : StickUpdateActivatedEtching
    {
        private readonly List<DamageEtching> _boostedEtchings = new List<DamageEtching>();
        private Stat _boostAmountStat;
        private int _boostAmount => _boostAmountStat.Value;

        protected override void Start()
        {
            _boostAmountStat = new Stat(design.GetStat(StatType.Boost));
            colorWhenActivated = false;
            BattleEvents.Current.OnEndBattle += ClearMods;
            base.Start();
        }

        private void ClearMods()
        {
            // Clear stored etchings
            foreach (var etching in _boostedEtchings)
            {
                etching.ModifyStat(StatType.Damage, -_boostAmount);
            }
            _boostedEtchings.Clear();
        }
        
        protected override bool TestStickMovementActivatedEffect()
        {
            ClearMods();

            if (deactivationTurns > 0) return false;
            
            var stickNumber = Stick.GetStickNumber();

            // Etching to the left
            if (stickNumber > 1)
            {
                var leftEtching = StickManager.current.GetStick(stickNumber - 1);
                if (leftEtching && leftEtching.etching)
                {
                    if (leftEtching.etching is DamageEtching etching)
                    {
                        _boostedEtchings.Add(etching);
                    }
                }
            }
            
            // Etching to the right
            if (stickNumber < StickManager.current.stickCount)
            {
                var rightEtching = StickManager.current.GetStick(stickNumber + 1);
                if (rightEtching && rightEtching.etching)
                {
                    if (rightEtching.etching is DamageEtching etching)
                    {
                        _boostedEtchings.Add(etching);
                    }
                }
            }

            if (_boostedEtchings.Count == 0) return false;

            foreach (var etching in _boostedEtchings)
            {
                etching.ModifyStat(StatType.Damage, +_boostAmount);
            }

            return true;
        }

        protected override bool CheckInfluence(int stickNum)
        {
            return (stickNum == Stick.GetStickNumber() - 1 || stickNum == Stick.GetStickNumber() + 1);
        }
        
    }
}

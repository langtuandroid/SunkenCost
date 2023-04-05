using System;
using System.Collections;
using System.Collections.Generic;
using Designs;
using Enemies;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Etchings
{
    public abstract class Etching : MonoBehaviour
    {
        public Design design;
        protected DesignDisplay designDisplay;
        
        protected bool colorWhenActivated = false;

        public Stick Stick { get; protected set; }
        protected int deactivationTurns;

        protected int UsesPerTurn => design.GetStat(StatType.UsesPerTurn);

        protected int UsesUsedThisTurn
        {
            get => design.UsesUsedThisTurn;
            set => design.UsesUsedThisTurn = value;
        }

        protected void Awake()
        {
            Stick = transform.parent.parent.GetComponent<Stick>();
            designDisplay = GetComponent<DesignDisplay>();
        }

        protected virtual void Start()
        {
            designDisplay.design = design;
            OldBattleEvents.Current.OnBeginPlayerTurn += designDisplay.Refresh;
        }

        protected int GetStatValue(StatType statType)
        {
            if (design.Stats.ContainsKey(statType)) return design.Stats[statType].Value;
            return -1;
        }

        protected abstract bool CheckInfluence(int stickNum);

        public void UpdateIndicators()
        {
            if (StickManager.current.IsDragging)
            {
                Stick.IndicatorController.Hide();
                return;
            }
            
            for (var i = 1; i < StickManager.current.stickCount; i++)
            {
                var stick = StickManager.current.GetStick(i);

                if (CheckInfluence(i))
                {
                    stick.IndicatorController.Show(design.Color);
                }
                else
                {
                    stick.IndicatorController.Hide();
                }
            }
        }

        private void OnDestroy()
        {
            OldBattleEvents.Current.OnBeginPlayerTurn -= designDisplay.Refresh;
        }
    }
}

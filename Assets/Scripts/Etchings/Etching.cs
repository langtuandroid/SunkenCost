using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Etchings
{
    public abstract class Etching : MonoBehaviour
    {
        public Design design;
        protected DesignInfo designInfo;
        
        protected bool colorWhenActivated = false;

        public Stick Stick { get; protected set; }
        protected int deactivationTurns;

        protected int UsesPerTurn => design.GetStat(St.UsesPerTurn);

        protected int UsesUsedThisTurn
        {
            get => design.UsesUsedThisTurn;
            set => design.UsesUsedThisTurn = value;
        }

        protected void Awake()
        {
            Stick = transform.parent.parent.GetComponent<Stick>();
            designInfo = GetComponent<DesignInfo>();
        }

        protected virtual void Start()
        {
            designInfo.design = design;
            BattleEvents.Current.OnBeginPlayerTurn += designInfo.Refresh;
        }

        protected int GetStatValue(St st)
        {
            if (design.Stats.ContainsKey(st)) return design.Stats[st].Value;
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
            BattleEvents.Current.OnBeginPlayerTurn -= designInfo.Refresh;
        }
    }
}

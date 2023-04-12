using System;
using UnityEngine;

namespace BattleScreen.BattleEvents
{
    [Serializable]
    public class UnexpectedBattleEventException : Exception
    {
        private BattleEventType _battleEventType;
        public override string Message => _battleEventType.ToString();

        public UnexpectedBattleEventException(BattleEvent battleEvent) : base()
        {
            _battleEventType = battleEvent.Type;
        }
    }
}
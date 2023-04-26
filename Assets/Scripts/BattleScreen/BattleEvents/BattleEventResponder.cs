using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleScreen.BattleEvents
{
    public abstract class BattleEventResponder : MonoBehaviour
    {
        public abstract BattleEventPackage GetResponseToBattleEvent(BattleEvent previousBattleEvent);
    }
}
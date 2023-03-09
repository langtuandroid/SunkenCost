using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class ExtraTurnItem : InBattleItem
    {
        protected override void Activate()
        {
            PlayerController.current.MovesPerTurn += 1;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class ExtraTurnItem : Item
    {
        protected override void Activate()
        {
            PlayerController.current.MovesPerTurn += 1;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTooltipTrigger : TooltipTrigger
{
    private Enemy _enemy;

    private void Awake()
    {
        _enemy = transform.parent.GetComponent<Enemy>();
    }
    
    protected override string GetContent()
    {
        var enemyMovement = "";
        
        if (_enemy.MoveMin == _enemy.MoveMax)
        {
            if (_enemy.MoveMin == 1)
            {
                enemyMovement = "1 plank ";
            }
            else
            {
                enemyMovement = _enemy.MoveMin + " planks ";
            }
        }
        else
        {
            enemyMovement = _enemy.MoveMin + "-" + _enemy.MoveMax + " planks ";
        }

        return "Moves " + enemyMovement + "per turn\n" + _enemy.GetDescription();
    }
}

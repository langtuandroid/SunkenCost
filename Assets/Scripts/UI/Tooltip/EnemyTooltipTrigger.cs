using System.Collections;
using System.Collections.Generic;
using Enemies;
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
        return _enemy.GetDescription();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyHealthText : EnemyUIText
{
    private int _health;
    private int _maxHealth;
    
    [SerializeField] private Color damagedColor;
    [SerializeField] private Color healingColor;

    private bool _justSpawned = true;
    
    
    public void AlterHealth(int health, int maxHealth)
    {
        if (!_justSpawned)
        {
            if (health > _health)
            {
                StartCoroutine(ChangeColorTemporarily(healingColor));
            }
            else if (health < _health)
            {
                StartCoroutine(ChangeColorTemporarily(damagedColor));
            }
        }
        else
        {
            _justSpawned = false;
        }

        _health = health;
        _maxHealth = maxHealth;
        SetText(_health + "/" + _maxHealth);
    }
}

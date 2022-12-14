using System;
using System.Collections;
using System.Collections.Generic;
using Etchings;
using UnityEngine;

public abstract class ActiveEtching : Etching
{
    private Color _normalColor;
    private CanvasGroup _canvasGroup;
    
    protected override void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        
        GameEvents.current.OnEndEnemyTurn += EndEnemyTurn;
        GameEvents.current.OnSticksUpdated += designInfo.RefreshCard;
        
        base.Start();
        _normalColor = designInfo.TitleText.color;
    }
    
    protected void EndEnemyTurn()
    {
        UsesUsedThisTurn = 0;

        if (deactivationTurns > 0)
        {
            deactivationTurns--;

            if (deactivationTurns == 0)
            {
                Stick.SetActive(true);
            }
        }
    }
    
    
    protected IEnumerator ColorForActivate()
    {
        designInfo.TitleText.color = Color.green;
        yield return new WaitForSeconds(BattleManager.AttackTime);
        designInfo.TitleText.color = _normalColor;
    }

    public void SetVisible(bool visible)
    {
        _canvasGroup.alpha = visible ? 1f : 0f;
    }
        

    public void Deactivate(int turns)
    {
        Stick.SetActive(false);
        deactivationTurns = turns;
    }

    private void OnDestroy()
    {
        GameEvents.current.OnEndEnemyTurn -= EndEnemyTurn;
        GameEvents.current.OnSticksUpdated -= designInfo.RefreshCard;
    }
}

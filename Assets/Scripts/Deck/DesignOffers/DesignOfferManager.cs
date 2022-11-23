using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesignOfferManager : MonoBehaviour
{
    public static DesignOfferManager current;
    [SerializeField] private GameObject designOfferPanel;
    private Canvas _canvas;

    private int _levelUp;

    private Transform _offerPanel;

    public bool isOffering;

    private void Awake()
    {
        if (current)
            Destroy(current.gameObject);
        current = this;
    }

    private void Start()
    {
        GameEvents.current.OnDesignOfferAccepted += DesignOfferAccepted;
        GameEvents.current.OnEndEnemyTurn += EndEnemyTurn;
        GameEvents.current.OnLevelUp += LevelUp;
        _canvas = FindObjectOfType<Canvas>();
    }

    private void CreateDesignPanel()
    {
        _offerPanel = Instantiate(designOfferPanel, _canvas.transform).transform;
        GameEvents.current.OfferDesigns();
        
        // Put behind tutorial
        _offerPanel.SetSiblingIndex(_offerPanel.parent.childCount - 3);
    }

    private bool CheckLevelUp()
    {
        if (_levelUp > 0)
        {
            CreateDesignPanel();
            _levelUp -= 1;
            return true;
        }

        return false;
    }

    private void DesignOfferAccepted()
    {
        Destroy(_offerPanel.gameObject);
        isOffering = CheckLevelUp();
    }

    private void EndEnemyTurn()
    {
        isOffering = CheckLevelUp();
    }

    private void LevelUp()
    {
        _levelUp++;
    }
}

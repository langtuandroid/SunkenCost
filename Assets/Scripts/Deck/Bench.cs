using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bench : MonoBehaviour
{
    public static Bench current;

    [SerializeField] private GameObject designCardPrefab;
    
    private readonly List<DesignCard> _cardsOnBench = new List<DesignCard>();

    private int _lastRemovedCardSiblingIndex;

    private void Awake()
    {
        if (current) Destroy(current);
        current = this;
    }

    private void UpdateDesigns()
    {
        var numOfCards = _cardsOnBench.Count;
        for (var i = 0; i < numOfCards; i++)
        {
            var numOfCardsFromCenter = i - (numOfCards / 2);

            var offsetPosition = numOfCardsFromCenter * 220;
            
            // Even number of cards
            if (numOfCards % 2 == 0)
            {
                offsetPosition += 110;
                
                // Just so the middle right card behaves like the middle left (no middle card)
                if (numOfCardsFromCenter >= 0) numOfCardsFromCenter += 1;
            }
            
            var offsetAngle = numOfCardsFromCenter * -3;

            var card = _cardsOnBench[i];
            card.transform.localPosition = new Vector2(offsetPosition, 0 - Mathf.Pow(Mathf.Abs(numOfCardsFromCenter)*2, 2.5f));
            card.transform.localRotation =  Quaternion.Euler(0, 0, offsetAngle);
            card.BenchUpdated();
        }
    }

    public void AddDesign(Design design)
    {
        var card = Instantiate(designCardPrefab, transform).AddComponent<DesignCard>();
        card.design = design;
        _cardsOnBench.Add(card);
        UpdateDesigns();
    }

    public void ReAddCard(DesignCard card)
    {
        card.transform.SetParent(transform);
        card.transform.SetSiblingIndex(_lastRemovedCardSiblingIndex);
        _cardsOnBench.Insert(_lastRemovedCardSiblingIndex - 1, card);
        UpdateDesigns();
    }

    private void RemoveCard(DesignCard card)
    {
        _lastRemovedCardSiblingIndex = card.transform.GetSiblingIndex();
        _cardsOnBench.Remove(card);
        UpdateDesigns();
    }

    public void DraggedCard(DesignCard card)
    {
        RemoveCard(card);
    }

    public List<Design> DiscardHand()
    {
        var designs = new List<Design>();
        foreach (var card in _cardsOnBench)
        {
            designs.Add(card.design);
            Destroy(card.gameObject);
        }
        
        _cardsOnBench.Clear();
        return designs;
    }
}

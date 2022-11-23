using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Deck : MonoBehaviour
{
    public static Deck current;
    
    private List<Design> _deck = new List<Design>();
    private List<Design> _drawPile = new List<Design>();
    private List<Design> _discardPile = new List<Design>();

    private int _drawAmount = 3;

    private void Awake()
    {
        if (current) Destroy(current);

        current = this;
    }

    private void Start()
    {
        AddDesignsToDrawPile(new List<Design>()
        {
            new Impede(),
            new Impede(),
            new Slinger(),
            new Slinger(),
            new Stab(),
            new Stab(),
            new Stomp(),
            new Stomp(),
        });

        _drawPile.Shuffle();
    }

    private void AddDesignsToDrawPile(List<Design> designs)
    {
        foreach (var design in designs)
        {
            AddDesignToDrawPile(design);
        }
    }

    public void AddDesignToDrawPile(Design design)
    {
        _drawPile.Add(design);
        AddDesign(design);
    }

    public void AddDesignToDiscardPile(Design design)
    {
        _discardPile.Add(design);
        AddDesign(design);
    }

    private void AddDesign(Design design)
    {
        if (_deck.Contains(design)) return;
        _deck.Add(design);
    }

    public void Redraw()
    {
        DiscardHand();
        DrawNewHand();
    }

    public void DrawNewHand()
    {
        for (var i = 0; i < _drawAmount; i++)
        {
            if (_drawPile.Count == 0) ShuffleDiscardPileIntoDrawPile();
            if (_drawPile.Count == 0) return;
            
            var design = _drawPile[0];
            _drawPile.RemoveAt(0);
            Bench.current.AddDesign(design);
        }
    }

    public void DiscardHand()
    {
        var discardedHand = Bench.current.DiscardHand();
        foreach (var design in discardedHand)
        {
            AddDesignToDiscardPile(design);
        }
    }

    private void ShuffleDiscardPileIntoDrawPile()
    {
        _drawPile.AddRange(_discardPile);
        _discardPile.Clear();
        _drawPile.Shuffle();
    }
}

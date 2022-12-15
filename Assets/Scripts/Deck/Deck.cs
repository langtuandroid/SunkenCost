using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public static class Deck
{
    private static List<Design> _designs = new List<Design>();

    public static List<Design> Designs
    {
        get => _designs;
        set => _designs = value;
    }

    static Deck()
    {
        _designs.Add(new Stab());
        _designs.Add(new Slinger());
        _designs.Add(new Impede());
    }
}

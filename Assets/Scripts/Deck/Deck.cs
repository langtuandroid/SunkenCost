using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public static class Deck
{
    private static List<Design> _designs = new List<Design>();

    public static List<Design> Designs
    {
        get
        {
            foreach (var design in _designs)
            {
                Debug.Log(design.Title);
            }

            return _designs;
        }
        set => _designs = value;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DesignOfferPanel : MonoBehaviour
{
    private Transform _grid;
    [SerializeField] private GameObject _designCardPrefab;
    [SerializeField] private GameObject _orText;
    private int _choices = 3;

    private void Awake()
    {
        _grid = transform.GetChild(1);
    }

    private void Start()
    {
        var designsOffered = new List<string>();
        var round = GameManager.current.Round;

        for (var i = 0; i < _choices; i++)
        {
            Type designClass;
            while (true)
            {
                var rarity = 0;

                if (round >= 10)
                {
                    var choices = new List<int>() {0, 0, 1};
                    rarity += choices[Random.Range(0, choices.Count)];
                }

                var newDesignChoices = DesignManager.Rarities.Where(r => r.Value == rarity).ToList();
                var newDesignName = newDesignChoices.ElementAt(Random.Range(0, (int)newDesignChoices.Count)).Key;
                
                // Already offered
                if (designsOffered.Contains(newDesignName)) continue;

                designClass = DesignManager.GetDesignType(newDesignName);

                designsOffered.Add(newDesignName);
                
                break;
            }
            var newDesign = Instantiate(_designCardPrefab, _grid).AddComponent<DesignCardOffer>();
            newDesign.design = (Design) Activator.CreateInstance(designClass);

            if (i < _choices - 1) Instantiate(_orText, _grid);
        }
    }
}

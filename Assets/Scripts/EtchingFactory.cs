using System;
using System.Collections.Generic;
using BattleScreen.BattleBoard;
using Designs;
using Etchings;
using UnityEngine;

public class EtchingFactory : MonoBehaviour
{
    public static EtchingFactory Current;
    
    [SerializeField] private GameObject _etchingPrefab;

    private void Awake()
    {
        if (Current)
            Destroy(Current.gameObject);

        Current = this;
    }

    public void CreateEtching(Plank plankDisplay, Design design)
    {
        var etchingSlot = plankDisplay.transform.GetChild(1);
        
        if (etchingSlot.childCount > 0)
        {
            // There's already an etching here!
            var occupyingEtchingTransform = etchingSlot.GetChild(0);
            Destroy(occupyingEtchingTransform.gameObject);
        }

        var newEtching = Instantiate(_etchingPrefab, etchingSlot);
        var etching = newEtching.AddComponent(DesignLoader.DesignAssetToEtchingTypeDict[design.designAsset])
            .GetComponent<Etching>();
        etching.design = design;
    }
}

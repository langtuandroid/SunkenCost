using System;
using System.Collections.Generic;
using BattleScreen.BattleBoard;
using Designs;
using Etchings;
using Loaders;
using Pickups.Varnishes;
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

    public void CreateEtching(Plank plank, Design design)
    {
        var etchingSlot = GetEtchingSlot(plank);
        
        if (etchingSlot.childCount > 0)
        {
            // There's already an etching here!
            var occupyingEtchingTransform = etchingSlot.GetChild(0);
            Destroy(occupyingEtchingTransform.gameObject);
        }

        var newEtching = Instantiate(_etchingPrefab, etchingSlot);
        var etching = newEtching.AddComponent(DesignLoader.DesignAssetToEtchingTypeDict[design.designAsset])
            .GetComponent<Etching>();
        etching.SetDesign(design);

        foreach (var varnishType in design.Varnishes)
        {
            var varnish = newEtching.AddComponent(VarnishLoader.GetVarnishByTypeEnum(varnishType)).GetComponent<Varnish>();
            varnish.Init(etching);
        }
    }

    public void MoveEtching(Plank plankToMoveTo, Etching etching)
    {
        var etchingSlot = GetEtchingSlot(plankToMoveTo);
        var etchingRectTransform = etching.GetComponent<RectTransform>();
        etchingRectTransform.SetParent(etchingSlot);
        etchingRectTransform.anchoredPosition = new Vector3(0, 0, 1);
        etching.SetPlank(plankToMoveTo);
    }

    private Transform GetEtchingSlot(Plank plank)
    {
        return plank.transform.GetChild(0).GetChild(1);
    }
}

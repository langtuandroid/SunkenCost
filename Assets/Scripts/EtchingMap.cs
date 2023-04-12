using System;
using System.Collections.Generic;
using Designs;
using Etchings;
using UnityEngine;

public class EtchingMap : MonoBehaviour
{
    public static EtchingMap Current;
    
    [SerializeField] private GameObject _etchingPrefab;
    
    public List<Etching> EtchingOrder { get; private set; } = new List<Etching>();

    private void Awake()
    {
        if (Current)
            Destroy(Current.gameObject);

        Current = this;
    }

    public void CreateEtching(Plank plank, Design design)
    {
        var etchingSlot = plank.transform.GetChild(1);
        
        if (etchingSlot.childCount > 0)
        {
            // There's already an etching here!
            var occupyingEtchingTransform = etchingSlot.GetChild(0);
            var occupyingEtching = occupyingEtchingTransform.GetComponent<Etching>();
            EtchingOrder.Remove(occupyingEtching);

            Destroy(occupyingEtchingTransform.gameObject);
        }

        var newEtching = Instantiate(_etchingPrefab, etchingSlot);
        var etching = newEtching.AddComponent(DesignManager.DesignAssetToEtchingTypeDict[design.designAsset])
            .GetComponent<Etching>();
        etching.design = design;
        etchingSlot.transform.parent.GetComponent<Plank>().Etching = etching;
    }

    public void RemoveEtching(Etching etching)
    {
        EtchingOrder.Remove(etching);
    }

    public void RefreshEtchingOrder()
    {
        var plankMap = PlankMap.Current;
        var newEtchingOrder = new List<Etching>();
        for (var i = 0; i < plankMap.PlankCount; i++)
        {
            var plank = plankMap.GetPlank(i);
            if (plank.Etching == null) continue;

            newEtchingOrder.Add(plank.Etching);
        }

        EtchingOrder = newEtchingOrder;
    }
}

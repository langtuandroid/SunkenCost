using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfferScreenEvents : MonoBehaviour
{
    public static OfferScreenEvents Current;

    public event Action OnGridsUpdated;

    private void Awake()
    {
        if (Current)
        {
            Destroy(Current.gameObject);
        }

        Current = this;
    }

    public void RefreshOffers()
    {
        OnGridsUpdated?.Invoke();
    }
}

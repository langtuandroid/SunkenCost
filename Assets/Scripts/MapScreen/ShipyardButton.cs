using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipyardButton : MonoBehaviour
{
    public void GoToShipYard()
    {
        MainManager.Current.LoadOfferScreen();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using MapScreen;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MapEventGenerator : MonoBehaviour
{
    [SerializeField] private List<Sprite> _mapEventSprites;
    /*
     * 0 : Coin
     * 1 : Heal
     * 2: Upgrade Card
     * 3: Specific Card
     */

    [SerializeField] private MapEvent topMapEvent;
    [SerializeField] private MapEvent bottomMapEvent;
    
    
    private static readonly Dictionary<MapEventType, float> Weightings = new Dictionary<MapEventType, float>()
    {
        {MapEventType.Coin, 0.4f},
        {MapEventType.Heart, 0.3f},
        {MapEventType.UpgradeCard, 0.2f},
        {MapEventType.SpecificCard, 0.1f}
    };

    private void Start()
    {
        var topEvent = GenerateEventType();
        UpdateEvent(topMapEvent, topEvent);

        MapEventType bottomEvent;
        while (true)
        {
            bottomEvent = GenerateEventType();
            if (bottomEvent != topEvent) break;
        }

        UpdateEvent(bottomMapEvent, bottomEvent);
        
        RunProgress.HasGeneratedMapEvents = true;
    }

    private void UpdateEvent(MapEvent mapEvent, MapEventType eventType)
    {
        mapEvent.EventType = eventType;

        var eventImage = mapEvent.transform.GetChild(2).GetComponent<Image>();

        switch (eventType)
        {
            case MapEventType.Coin:
                eventImage.sprite = _mapEventSprites[0];
                mapEvent.UpdateDescription("+3 extra coins");
                break;
            case MapEventType.Heart:
                eventImage.sprite = _mapEventSprites[1];
                mapEvent.UpdateDescription("Heal 1 life");
                break;
            case MapEventType.UpgradeCard:
                eventImage.sprite = _mapEventSprites[2];
                mapEvent.UpdateDescription("Upgrade a card");
                break;
            case MapEventType.SpecificCard:
                eventImage.sprite = _mapEventSprites[3];
                mapEvent.UpdateDescription("Find rare card");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private MapEventType GenerateEventType()
    {
        var sequence = new[] {
            MapEventType.Coin,
            MapEventType.Heart,
            MapEventType.UpgradeCard,
            MapEventType.SpecificCard
        };
        
        var rand = Random.value;

        foreach (var mapEvent in sequence)
        {
            var weighting = Weightings[mapEvent];
                
            if (rand <= weighting)
                return mapEvent;

            rand -= weighting;
        }

        Debug.Log("Error: no weighting found for " + rand);
        return MapEventType.None;
    }
}

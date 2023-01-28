using System.Collections.Generic;
using UnityEngine;

namespace MapScreen
{
    public class MapGenerator : MonoBehaviour
    {
        private static readonly Dictionary<MapEvent, float> Weightings = new Dictionary<MapEvent, float>()
        {
            {MapEvent.Battle, 0.5f},
            {MapEvent.EliteBattle, 0.08f},
            {MapEvent.Shop, 0.17f},
            {MapEvent.Mystery, 0.25f}
        };

        private const float ChanceOfNoEvent = 0.3f;

        private void Start()
        {
            GenerateNewMap();
        }

        public static void GenerateNewMap()
        {
            var map = new Map();

            for (var x = 0; x < Map.SizeX; x++)
            {
                for (var y = 0; y < Map.SizeY; y++)
                {
                    if (map.GetMapEvent(x, y) != MapEvent.None)
                    {
                        Debug.Log("Already map event on [" + x + ", " + y + "]");
                    }
                    else
                    {
                        map.SetMapEvent(x, y, GenerateEvent(map, x, y));
                    }
                    
                    Debug.Log(map.GetMapEvent(x, y));
                }
            }
            
            
        }

        private static MapEvent GenerateEvent(Map map, int x, int y)
        {
            var rand = Random.value;

            if (rand <= ChanceOfNoEvent)
                return MapEvent.None;

            if (
                map.GetMapEvent(Mathf.Clamp(x - 1, 0, Map.SizeX - 1), y) != MapEvent.None ||
                map.GetMapEvent(Mathf.Clamp(x + 1, 0, Map.SizeX - 1), y) != MapEvent.None ||
                map.GetMapEvent(x, Mathf.Clamp(y - 1, 0, Map.SizeY - 1)) != MapEvent.None ||
                map.GetMapEvent(x, Mathf.Clamp(y + 1, 0, Map.SizeY - 1)) != MapEvent.None
            )
            {
                return MapEvent.None;
            }

            rand = Random.value;

            if (rand <= Weightings[MapEvent.Battle])
            {
                return MapEvent.Battle;
            }
            
            rand -= Weightings[MapEvent.Battle];
            if (rand <= Weightings[MapEvent.EliteBattle])
            {
                return MapEvent.EliteBattle;
            }
            
            rand -= Weightings[MapEvent.EliteBattle];
            if (rand <= Weightings[MapEvent.Shop])
            {
                return MapEvent.Shop;
            }
            
            rand -= Weightings[MapEvent.Shop];
            if (rand <= Weightings[MapEvent.Mystery])
            {
                return MapEvent.Mystery;
            }
            
            Debug.Log("Error: no weighting found for " + rand);
            return MapEvent.None;
        }
    }
}

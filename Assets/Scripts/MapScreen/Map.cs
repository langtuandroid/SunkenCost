using System.Drawing;
using UnityEngine;

/*
namespace MapScreen
{
    public enum OldMapEventType
    {
        None,
        Battle,
        EliteBattle,
        Booty,
        Shop,
        Mystery
    }
    
    public class Map
    {
        public const int SizeX = 10;
        public const int SizeY = 6;
        
        // The map itself
        public OldMapEventType[,] MapEventCoords { get; private set; } = new OldMapEventType[SizeX, SizeY];

        public Map()
        {
            for (var x = 0; x < SizeX; x++)
            {
                for (var y = 0; y < SizeY; y++)
                {
                    SetMapEvent(x, y, OldMapEventType.None);
                }
            }
        }
        
        public void SetMapEvent(int positionX, int positionY, OldMapEventType oldMapEvent)
        {
            if (positionX >= SizeX || positionY >= SizeY)
            {
                Debug.Log("Cannot set grid, position is out of bounds. [" + positionX + ", " + positionY + "]");
                return;
            }
            
            MapEventCoords[positionX, positionY] = oldMapEvent;
        }

        public OldMapEventType GetMapEvent(int positionX, int positionY)
        {
            if (positionX >= SizeX || positionY >= SizeY)
            {
                Debug.Log("Cannot get event, position is out of bounds: [" + positionX + ", " + positionY + "]");
                return OldMapEventType.None;
            }
            
            return MapEventCoords[positionX, positionY];
        }
    }
}
*/
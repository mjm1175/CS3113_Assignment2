using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public List<Room> Rooms;

    private void Awake()
    {
        // If the game already exists
        if (PublicVars.Game != null)
        {
            Destroy(gameObject);
            return;
        DontDestroyOnLoad(this);
        //Room start = new Room("SampleScene");
        //Room path0 = new Room("Path0");
        //Room path2 = new Room("Path2", prereqRooms: new Room[] { start });

        Room start = new Room("Corridor");
        Room path0 = new Room("Room1");
        Room path1 = new Room("Room2", prereqRooms: new Room[] { start });
        Room path2 = new Room("Room3", prereqRooms: new Room[] { start });
        
        Room boss = new Room("BossScene", prereqRooms: new Room[] { start, path0, path2 });

        Rooms = new List<Room>() { start, path0, path1, path2, boss };

        PublicVars.Game = this;
        start.Enter();
    }
}

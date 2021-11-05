using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public List<Room> Rooms;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        Room start = new Room("SampleScene");
        Room path0 = new Room("Path0");
        Room path2 = new Room("Path2", prereqRooms: new Room[] { start });
        Room boss = new Room("BossScene", prereqRooms: new Room[] { start, path0, path2 });

        Rooms = new List<Room>() { start, path0, path2, boss };

        PublicVars.Game = this;
        start.Enter();
    }
}

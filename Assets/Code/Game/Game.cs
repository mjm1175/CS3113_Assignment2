using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private void Awake()
    {
        // If the game already exists
        if (PublicVars.Game != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);

        // Initialize the rooms

        Room start = new Room("Corridor");
        Room cell = new Room("Cell");
        Room c1 = new Room("Corridor1");
        Room c2 = new Room("Corridor2");
        Room c3 = new Room("Corridor3");

        List<string> roomIds = new List<string>();

        for (int i = 1; i <= 8; i++)
        {
            roomIds.Add($"Room{i}");
        }

        Room lastNormalRoom = Room.RegisterSequentialRooms(roomIds, new Room[] { start });

        Room boss = new Room("BossScene", prereqRooms: new Room[] { lastNormalRoom });

        PublicVars.Game = this;
        cell.Enter();
        //start.Enter();
    }
}

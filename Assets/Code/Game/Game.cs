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

        Room start = new Room("Corridor", 4);
        Room cell = new Room("Cell", 1);
        Room c1 = new Room("Corridor1");
        Room c2 = new Room("Corridor2");
        Room c3 = new Room("Corridor3");
        Room c4 = new Room("Corridor4");

        cell.Connect(start, 0, 0);
        start.Connect(c1, 1, 0);
        start.Connect(c2, 2, 0);
        start.Connect(c3, 3, 0);

        List<string> roomIds = new List<string>();

        for (int i = 1; i <= 11; i++)
        {
            roomIds.Add($"Room{i}");
        }

        Room lastNormalRoom = Room.RegisterSequentialRooms(roomIds);

        Room boss = new Room("BossScene");

        PublicVars.Game = this;
        cell.Enter();
        //start.Enter();
    }
}

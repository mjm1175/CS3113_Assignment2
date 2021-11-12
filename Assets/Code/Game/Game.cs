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
        }
        DontDestroyOnLoad(this);

        Room start = new Room("Corridor");

        List<string> roomIds = new List<string>();

        for (int i = 1; i <= 8; i++)
        {
            roomIds.Add($"Room{i}");
        }

        Room lastNormalRoom = Room.RegisterSequentialRooms(roomIds, new Room[] { start });

        Room boss = new Room("BossScene", prereqRooms: new Room[] { lastNormalRoom });

        PublicVars.Game = this;
        start.Enter();
    }
}

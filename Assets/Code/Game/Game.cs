using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public void SetupRooms()
    {
        Room.Reset();
        // Initialize the rooms

        Room start = new Room("Corridor", 4, true);
        Room cell = new Room("Cell", 1, true);
        Room c1 = new Room("Corridor1", 7, true);
        Room c2 = new Room("Corridor2", 6, true);
        Room c3 = new Room("Corridor3", 6, true);
        Room c4 = new Room("Corridor4", 4, true);
        Room boss = new Room("Boss", 1);

        Room rm3 = new Room("Room3");

        // Connect the cell to the starting corridor
        // and the starting corridor to the secondary corridors
        cell.Connect(start, 0);
        start.Connect(c1, 1);
        start.Connect(c2, 2);
        start.Connect(c3, 3);

        // Connect the rooms in c1
        //Room.BulkConnectRooms(c1, 1, new string[] { "Room1", "Room2", null, "Room3", "Room4", "Room5" });
        Room.BulkConnectRooms(c1, 4, new string[] { "Room1", "Room4", "Room3" });
        c1.Connect("Room11", 1, 0);
        c1.Connect("Room11", 2, 1);
        c1.Connect(c4, 3, 0);

        // Connect Room3 to Room6
        rm3.Connect("Room6", 1, 0);

        // Connect the rooms in c2
        //Room.BulkConnectRooms(c2, 1, new string[] { "Room6", "Room7", null, "Room8", "Room9" });
        Room.BulkConnectRooms(c2, 4, new string[] { "Room5", "Room10" });
        c2.Connect("Room8", 1, 0);
        c2.Connect("Room8", 2, 1);
        c2.Connect(c4, 3, 1);

        // Connect the rooms in c3
        //Room.BulkConnectRooms(c3, 1, new string[] { "Room10", "Room11", null, "Room12", "Room13" });
        Room.BulkConnectRooms(c3, 4, new string[] { "Room12", "Room13" });
        c3.Connect("Room9", 1, 0);
        c3.Connect("Room9", 2, 1);
        c3.Connect(c4, 3, 2);

        // Connect the rooms in c4
        c4.Connect(boss, 3);

        try
        {
            Room.Enter(SceneManager.GetActiveScene().name);
        }
        catch (ArgumentException)
        {
            Debug.LogWarning($"{SceneManager.GetActiveScene().name} is not a existing room");
        }
    }

    public void InitializeGame()
    {
        SetupRooms();
        PublicVars.Reset(true);
    }

    private void Awake()
    {
        // If the game already exists
        if (PublicVars.Game != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        PublicVars.Game = this;

        InitializeGame();
    }
}

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

        start.CameraConfig = CineConfig.Down;

        Room rm2 = new Room("Room2");
        Room rm3 = new Room("Room3");
        Room rm8 = new Room("Room8");

        // Connect the cell to the starting corridor
        // and the starting corridor to the secondary corridors
        cell.Connect(start, 0);
        start.Connect(c1, 1);
        start.Connect(c2, 2);
        start.Connect(c3, 3);

        // Connect the rooms in c1
        //Room.BulkConnectRooms(c1, 1, new string[] { "Room1", "Room2", null, "Room3", "Room4", "Room5" });
        Room.BulkConnectRooms(c1, 4, new string[] { "Room1", null, "Room3" });
        rm2.Connect("Room11", 0, 0);
        c1.Connect("Room7", 1, 0);
        c1.Connect("Room11", 2, 1);
        c1.Connect(c4, 3, 0);

        // Connect Room3 to Room6
        rm3.Connect("Room6", 1, 0);

        // Room8 connects c1 and c2
        rm8.Connect(c2, 0, 1);
        rm8.Connect(c1, 1, 5);

        // Connect the rooms in c2
        //Room.BulkConnectRooms(c2, 1, new string[] { "Room6", "Room7", null, "Room8", "Room9" });
        Room.BulkConnectRooms(c2, 4, new string[] { "Room5", "Room10" });
        c2.Connect("Room4", 2, 0);
        c2.Connect(c4, 3, 1);

        // Connect the rooms in c3
        //Room.BulkConnectRooms(c3, 1, new string[] { "Room10", "Room11", null, "Room12", "Room13" });
        Room.BulkConnectRooms(c3, 2, new string[] { "Room12", null, null, "Room13" });
        c3.Connect("Room9", 1, 0);
        c3.Connect("Room9", 4, 1);
        c3.Connect(c4, 3, 2);

        // Connect the rooms in c4
        c4.Connect(boss, 3, 0);

        Room.FindRoomById("Room1").CameraConfig = CineConfig.Right;
        rm2.CameraConfig = CineConfig.Left;
        rm3.CameraConfig = CineConfig.Right;
        Room.FindRoomById("Room6").CameraConfig = CineConfig.Right;
        Room.FindRoomById("Room7").CameraConfig = CineConfig.Left;
        Room.FindRoomById("Room8").CameraConfig = CineConfig.Down;
        Room.FindRoomById("Room9").CameraConfig = CineConfig.Down;
        Room.FindRoomById("Room10").CameraConfig = CineConfig.Right;
        Room.FindRoomById("Room12").CameraConfig = CineConfig.Left;
        Room.FindRoomById("Room13").CameraConfig = CineConfig.Right;
        boss.CameraConfig = CineConfig.Left;

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

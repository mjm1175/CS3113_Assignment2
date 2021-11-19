using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    TransitionManager _transitionManager;

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
        Room boss = new Room("BossScene", 1);

        // Connect the cell to the starting corridor
        // and the starting corridor to the secondary corridors
        cell.Connect(start, 0);
        start.Connect(c1, 1);
        start.Connect(c2, 2);
        start.Connect(c3, 3);

        // Connect the rooms in c1
        Room.BulkConnectRooms(c1, 1, new string[] { "Room1", "Room2", null, "Room3", "Room4", "Room5" });
        c1.Connect(c4, 3, 0);

        // Connect the rooms in c2
        Room.BulkConnectRooms(c2, 1, new string[] { "Room6", "Room7", null, "Room8", "Room9" });
        c2.Connect(c4, 3, 1);

        // Connect the rooms in c3
        Room.BulkConnectRooms(c3, 1, new string[] { "Room10", "Room11", null, "Room12", "Room13" });
        c3.Connect(c4, 3, 2);

        // Connect the rooms in c4
        c4.Connect(boss, 3);

        PublicVars.Game = this;
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
        PublicVars.Reset();
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

        InitializeGame();
    }

    void Start()
    {
        _transitionManager = FindObjectOfType<TransitionManager>();
    }
}

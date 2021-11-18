using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    TransitionManager _transitionManager;
    
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
        Room c1 = new Room("Corridor1", 7);
        Room c2 = new Room("Corridor2");
        Room c3 = new Room("Corridor3");
        Room c4 = new Room("Corridor4");

        cell.Connect(start, 0);
        start.Connect(c1, 1);
        start.Connect(c2, 2);
        start.Connect(c3, 3);

        Room.BulkConnectRooms(c1, 1, new string[] { "Room1", "Room2", "Room3", "Room4", "Room5" });
        c1.Connect(c4, 6);

        List<string> roomIds = new List<string>();

        for (int i = 6; i <= 11; i++)
        {
            roomIds.Add($"Room{i}");
        }

        Room lastNormalRoom = Room.RegisterSequentialRooms(roomIds);

        Room boss = new Room("BossScene");

        PublicVars.Game = this;
        Room.Enter(SceneManager.GetActiveScene().name);        
    }
    void Start()
    {
        _transitionManager = FindObjectOfType<TransitionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //_transitionManager.LoadScene("Start");
    }
}

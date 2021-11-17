using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManualRoomPath : MonoBehaviour
{
    Scene curr_scene;
    public Dictionary<string, List<string>> corridorList = new Dictionary<string, List<string>>();
    public Dictionary<string, string> roomList;
    public Dictionary<string, bool> gotKey;
    void Start()
    {
        corridorList.Add("Corridor1", new List<string> { "Room3", "Room6", "Room4" });
        corridorList.Add("Corridor2", new List<string> { "Room1", "Room8", "Room7" });
        corridorList.Add("Corridor3", new List<string> { "Room2", "Room9", "Room10" });
        corridorList.Add("Corridor2", new List<string> { "Room5", "Room", "Room11" });

        foreach (KeyValuePair<string, List<string>> rooms in corridorList)
        {
            roomList.Add(rooms.Value[0], rooms.Key);
            roomList.Add(rooms.Value[1], rooms.Key);
            roomList.Add(rooms.Value[2], rooms.Key);

            gotKey.Add(rooms.Value[0], false);
            gotKey.Add(rooms.Value[1], false);
            gotKey.Add(rooms.Value[2], false);

            gotKey.Add(rooms.Key, true);
        }

    }

    public void LoadNextRoom(string currRoom, int doorNum)
    {
        if (roomList[currRoom] != null)
        {
            SceneManager.LoadScene(roomList[currRoom]);
        }
        else if (corridorList[currRoom] != null)
        {
            SceneManager.LoadScene(corridorList[currRoom][doorNum]);
        }
    }

    public void setKey(string currRoom, bool ifGotKey)
    {
        gotKey[currRoom] = ifGotKey;
    }

    public bool getKey(string currRoom)
    {
        return gotKey[currRoom];
    }

}

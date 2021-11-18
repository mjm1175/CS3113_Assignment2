using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomClass : MonoBehaviour
{
    public Dictionary<string, (string, string, string)> corridorList;
    public Dictionary<string, string> roomList;
    void Start()
    {
        //roomlist.Add("Room1", "Room2");
        corridorList.Add("Corridor1", ("Room3", "Room6", "Room4"));
        corridorList.Add("Corridor2", ("Room1", "Room8", "Room7"));
        corridorList.Add("Corridor3", ("Room2", "Room9", "Room10"));
        corridorList.Add("Corridor4", ("Corridor1", "Corridor2", "Corridor3"));

        //foreach ((string key, string val1, string val2, string val3) in corridorList) {

        //}
    }
}

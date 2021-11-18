using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revisit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Room currentRoom = Room.CurrentRoom;
        if (currentRoom.IsCompleted){
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}

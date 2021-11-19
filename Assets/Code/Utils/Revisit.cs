using System.Linq;
using UnityEngine;

public class Revisit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Room currentRoom = Room.CurrentRoom;
        if (currentRoom == null || currentRoom.IsCompleted)
        {
            gameObject.SetActive(false);
            return;
        };
    }
}

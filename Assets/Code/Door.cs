using UnityEngine;

public class Door : MonoBehaviour
{
    public string levelToLoad;
    public bool locked = true;
    public int doorCode = 0;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !locked)
        {
            Room.CurrentRoom.Complete().FindNextRoom().Enter();
        }
    }

}

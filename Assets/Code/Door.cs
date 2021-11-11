using UnityEngine;

public class Door : MonoBehaviour
{
    public string levelToLoad;
    public bool locked = true;
    public int doorCode = 0;
    public GameObject player;

    private void Update()
    {
        if (PublicVars.got_key == true)
        {
            locked = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !locked)
        {
            Room.CurrentRoom.Complete().FindNextRoom().Enter();
            PublicVars.got_key = false;
            //other.gameObject.GetComponent<T>().gotKey = false;
        }
    }

}

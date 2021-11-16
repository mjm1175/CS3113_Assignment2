using UnityEngine;

public class Door : MonoBehaviour
{
    public string levelToLoad;
    public bool locked = true;
    public int doorCode = 0;
    public GameObject player;

    private Room _currentRoom;

    private void Start()
    {
        _currentRoom = Room.CurrentRoom;
    }

    private void OnTriggerEnter(Collider other)
    {
        Inventory inventory;
        if (other.TryGetComponent<Inventory>(out inventory))
        {
            if (inventory.CheckItem(ItemType.Key, doorCode))
            {
                locked = false;
            }
        }

        if (other.gameObject.CompareTag("Player") && !locked)
        {
            PublicVars.got_key = false;
            _currentRoom.Complete().FindNextRoom().Enter();
        }
    }

}

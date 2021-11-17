using UnityEngine;

public class Door : MonoBehaviour
{
    [Tooltip("When unset, we will proceed to the next room by default; this hardcodes the next room to enter by id otherwise")]
    public string RoomToLoad;
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
            _currentRoom.Complete();

            if (RoomToLoad.Length > 0)
                Room.Enter(RoomToLoad);
            else
                _currentRoom.FindNextRoom().Enter();
        }
    }

}

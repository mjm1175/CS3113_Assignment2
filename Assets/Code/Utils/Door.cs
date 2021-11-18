using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    [Tooltip("When unset, we will proceed to the next room by default; this hardcodes the next room to enter by id otherwise")]
    public string RoomToLoad;
    public bool locked = true;
    public int doorIndex = 0;
    public int numPapers = 0;
    //public GameObject player;

    private Room _currentRoom;
    public Text lockedText = null;

    private void Start()
    {
        _currentRoom = Room.CurrentRoom;
        if (lockedText) lockedText.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // don't think we are using keys for doors anymore
        Inventory inventory;
        if (other.TryGetComponent<Inventory>(out inventory))
        {
            if (inventory.CheckItem(ItemType.Key, doorIndex))
            {
                locked = false;
            }
        }

        if (other.gameObject.CompareTag("Player"))
        {
            if (!locked || (PublicVars.paper_count >= numPapers))
            {
                _currentRoom.Complete();

                if (RoomToLoad.Length > 0)
                    Room.Enter(RoomToLoad);
                else
                    _currentRoom.EnterDoor(doorIndex);
            }
            else
            {
                lockedText.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lockedText.enabled = false;
        }
    }

}

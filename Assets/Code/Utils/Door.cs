using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    [Tooltip("When unset, we will proceed to the next room by default; this hardcodes the next room to enter by id otherwise")]
    public string RoomToLoad;
    public bool locked = true;
    public int doorIndex = 0;
    public int numPapers = 0;

    [Tooltip("The position where we will spawn the player when entering this door")]
    public Vector3 SpawnOffset;

    public Text lockedText = null;

    private void Start()
    {
        if (lockedText) lockedText.enabled = false;
        if (PublicVars.LastEnteredDoorIndex == doorIndex)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && SpawnOffset != null) player.transform.position = transform.position + SpawnOffset;
        }
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
            AudioSource doorOpening = other.gameObject.GetComponent<AudioSource>();
            if (!locked || (PublicVars.paper_count >= numPapers))
            {
                Room currentRoom = Room.CurrentRoom;
                currentRoom.Complete();

                if (RoomToLoad.Length > 0)
                    Room.Enter(RoomToLoad);
                else
                    currentRoom.EnterDoor(doorIndex);
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

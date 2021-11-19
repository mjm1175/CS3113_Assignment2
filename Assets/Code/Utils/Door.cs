using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    public bool locked = true;
    public int doorIndex = 0;
    public int numPapers = 0;

    [Tooltip("The position where we will spawn the player when entering this door")]
    public Vector3 SpawnOffset;

    public Text lockedText = null;

    private bool _entered;
    private Revisit _revisit;

    private void Start()
    {
        _revisit = GameObject.FindObjectOfType<Revisit>();
        if (lockedText) lockedText.enabled = false;
        if (PublicVars.LastEnteredDoorIndex == doorIndex)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && SpawnOffset != null) player.transform.position = transform.position + SpawnOffset;
        }
        _entered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_entered) return;
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
            if (!locked || (PublicVars.PaperCount >= numPapers))
            {
                _entered = true;

                Room currentRoom = Room.CurrentRoom;
                if (currentRoom == null)
                {
                    Debug.LogWarning("Cannot load the current room");
                    return;
                }
                if (_revisit != null)
                {
                    ICollectable[] children = _revisit.GetComponentsInChildren<ICollectable>();
                    if (children == null || children.Length == 0)
                        currentRoom.Complete();
                }
                else
                {
                    currentRoom.Complete();
                }
                currentRoom.EnterDoor(doorIndex);
            }
            else
            {
                if (lockedText != null)
                {
                    lockedText.enabled = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (lockedText != null)
            {
                lockedText.enabled = false;
            }
        }
    }

}

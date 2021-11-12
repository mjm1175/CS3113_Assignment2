using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Collectable : MonoBehaviour
{
    [Tooltip("The name (and the display name) of the collectable")]
    public string Name;
    [Tooltip("If true, the item will be persisted when travelling to a different room")]
    public bool IsPersistent;
    [Tooltip("The type of the item, regular by default")]
    public ItemType ItemType;
    [Tooltip("Reserved for specially purposed items like keys")]
    public int Data;

    private void OnTriggerEnter(Collider other)
    {
        Inventory inventory;
        if (other.TryGetComponent<Inventory>(out inventory))
        {
            inventory.AddItem(new Item(Name, IsPersistent, ItemType, Data));
            Destroy(gameObject);
        }
    }
}
/// <summary>
/// An abstraction for all the collectable items
/// </summary>
public struct Item
{
    /// <value>The name (and the display name) of the collectable</value>
    public string Name;
    /// <value>If true, the item will be persisted when travelling to a different room</value>
    public bool IsPersistent;
    /// <value>The type of the item, regular by default</value>
    public ItemType ItemType;
    /// <value>Reserved for specially purposed items like keys</value>
    public int Data;

    public Item(string name, bool isPersistent, ItemType itemType = ItemType.Regular, int data = 0)
    {
        Name = name;
        IsPersistent = isPersistent;
        ItemType = itemType;
        Data = data;
    }
}
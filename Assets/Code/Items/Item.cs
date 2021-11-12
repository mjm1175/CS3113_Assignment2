/// <summary>
/// An abstraction for all the collectable items
/// </summary>
public class Item
{
    /// <value>The name (and the display name) of the collectable</value>
    public string Name { get; protected set; }
    /// <value>If true, the item will be persisted when travelling to a different room</value>
    public bool IsPersistent { get; protected set; }
    /// <value>The type of the item, regular by default</value>
    public ItemType ItemType { get; protected set; }
    /// <value>Reserved for specially purposed items like keys</value>
    public int Data { get; set; }

    public Item(string name, bool isPersistent, ItemType itemType = ItemType.Regular, int data = 0)
    {
        Name = name;
        IsPersistent = IsPersistent;
        ItemType = itemType;
        Data = data;
    }
}
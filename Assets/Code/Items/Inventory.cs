using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> Items { get; private set; }

    private void Start()
    {
        Items = new List<Item>();
    }

    public void AddItem(Item item)
    {
        Items.Add(item);
    }

    public bool CheckItem(ItemType expectedType, int expectedData)
    {
        return Items.Any(item => item.ItemType == expectedType && item.Data == expectedData);
    }

    public bool CheckItem(ItemType expectedType)
    {
        return Items.Any(item => item.ItemType == expectedType);
    }
}
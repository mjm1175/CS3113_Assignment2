using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Instances of Inventory will always populate items stored in PublicVars upon starting</summary>
public class Inventory : MonoBehaviour
{
    private List<Item> _items;

    private void Start()
    {
        _items = new List<Item>();
        PublicVars.Items.ForEach(item => _items.Add(item));
    }

    public void AddItem(Item item)
    {
        _items.Add(item);
        if (item.IsPersistent) PublicVars.Items.Add(item);
    }

    public bool CheckItem(ItemType expectedType, int expectedData)
    {
        return _items.Any(item => item.ItemType == expectedType && item.Data == expectedData);
    }

    public bool CheckItem(ItemType expectedType)
    {
        return _items.Any(item => item.ItemType == expectedType);
    }
}
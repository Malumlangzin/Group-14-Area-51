using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<Item> items = new();

    public event Action<string, Item> OnItemAdded;
    public event Action<string> OnItemRemoved;

    public void AddItem(Item item)
    {
        string id = Guid.NewGuid().ToString();
        items.Add(item);
        OnItemAdded?.Invoke(id, item);
    }

    public void RemoveItem(string inventoryId)
    {
        OnItemRemoved?.Invoke(inventoryId);
    }

    public void DropItem(string inventoryId)
    {
        Debug.Log($"Drop requested for item UI id: {inventoryId}");
        RemoveItem(inventoryId);
    }

    public List<Item> GetItems() => items;
}

using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject uiItemPrefab;

    [Header("References")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private Transform uiInventoryParent; 

    private readonly Dictionary<string, ItemUI> inventoryUI = new();

    private void OnEnable()
    {
        if (inventory != null)
        {
            inventory.OnItemAdded += AddUIItem;
            inventory.OnItemRemoved += RemoveUIItem;
        }
        ItemUI.OnItemSelected += HandleItemSelected;
    }

    private void OnDisable()
    {
        if (inventory != null)
        {
            inventory.OnItemAdded -= AddUIItem;
            inventory.OnItemRemoved -= RemoveUIItem;
        }
        ItemUI.OnItemSelected -= HandleItemSelected;
    }

    private void AddUIItem(string inventoryId, Item item)
    {
        if (uiItemPrefab == null || uiInventoryParent == null) return;

        var go = Instantiate(uiItemPrefab, uiInventoryParent);
        var itemUI = go.GetComponent<ItemUI>();
        if (itemUI == null)
        {
            Debug.LogError("uiItemPrefab is missing ItemUI component.");
            Destroy(go);
            return;
        }

        itemUI.Initialize(inventoryId, item, inventory.DropItem);
        inventoryUI[inventoryId] = itemUI;
    }

    private void RemoveUIItem(string inventoryId)
    {
        if (inventoryUI.TryGetValue(inventoryId, out var itemUI))
        {
            Destroy(itemUI.gameObject);
            inventoryUI.Remove(inventoryId);
        }
    }

    private void HandleItemSelected(int itemID, Sprite icon, string itemName)
    {
        InventoryWheelController.Instance?.UpdateSelectedItem(itemID, icon, itemName);
    }
}

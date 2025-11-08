using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryMenu;
    private bool menuActive;
    public ItemSlot[] ItemSlot;

    [SerializeField] private List<Tools> allTools = new List<Tools>();

    private void Awake()
    {
        inventoryMenu.SetActive(false);
    }

    public void OnToggleInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            menuActive = !menuActive;
            inventoryMenu.SetActive(menuActive);
            Time.timeScale = menuActive ? 0f : 1f;

            Cursor.visible = menuActive;
            Cursor.lockState = menuActive ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    public void AddItem(string itemName, int quantity, Sprite itemIcon)
    {
        for (int  i = 0;  i < ItemSlot.Length;  i++)
        {
            if (ItemSlot[i].isFull == false)
            {   ItemSlot[i].AddItem(itemName, quantity, itemIcon);
                return;
            }
        }

    }

    public void DeselectAllSlots()
        {
        for (int i = 0; i < ItemSlot.Length; i++)
        {
     
            ItemSlot[i].selectedShader.SetActive(false);
            ItemSlot[i].isSelected = false;
        }
    }

    public GameObject GetPrefabForItem(string itemName)
    {
        foreach (var tool in allTools) 
        {
            if (tool.Id == itemName)
                return tool.Prefab;
        }
        return null;
    }

}


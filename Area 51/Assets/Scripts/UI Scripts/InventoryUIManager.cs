using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class ToolReference
{
    public string Id;
    public GameObject Prefab;
}

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryMenu;
    [SerializeField] private List<ToolReference> allTools = new List<ToolReference>();
    public ItemSlot[] ItemSlot;

    private bool menuActive;
    private int currentSlotIndex = 0;

    private void Awake()
    {
        inventoryMenu.SetActive(false);
    }

    public void OnToggleInventory(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        menuActive = !menuActive;
        inventoryMenu.SetActive(menuActive);
        Time.timeScale = menuActive ? 0f : 1f;

        Cursor.visible = menuActive;
        Cursor.lockState = menuActive ? CursorLockMode.None : CursorLockMode.Locked;

        if (menuActive)
        {
            currentSlotIndex = 0;
            HighlightCurrentSlot();
        }
        else
        {
            DeselectAllSlots();
        }
    }

    public void AddItem(string itemName, int quantity, Sprite itemIcon)
    {
        foreach (var slot in ItemSlot)
        {
            if (!slot.isFull)
            {
                slot.AddItem(itemName, quantity, itemIcon);
                return;
            }
        }
    }

    public void DeselectAllSlots()
    {
        foreach (var slot in ItemSlot)
        {
            slot.selectedShader.SetActive(false);
            slot.isSelected = false;
        }
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (!menuActive || !context.performed) return;

        Vector2 input = context.ReadValue<Vector2>();

        if (input.x > 0.5f)
            MoveSelection(1);
        else if (input.x < -0.5f)
            MoveSelection(-1);
    }

    public void OnSelectItem(InputAction.CallbackContext context)
    {
        if (menuActive && context.performed)
            ItemSlot[currentSlotIndex].OnRightClickInput();
    }

    public void OnDropItem(InputAction.CallbackContext context)
    {
        if (menuActive && context.performed)
            ItemSlot[currentSlotIndex].OnLeftClickInput();
    }

    private void MoveSelection(int direction)
    {
        DeselectAllSlots();

        currentSlotIndex += direction;

        if (currentSlotIndex >= ItemSlot.Length)
            currentSlotIndex = 0;
        else if (currentSlotIndex < 0)
            currentSlotIndex = ItemSlot.Length - 1;

        HighlightCurrentSlot();
    }

    private void HighlightCurrentSlot()
    {
        DeselectAllSlots();
        ItemSlot[currentSlotIndex].selectedShader.SetActive(true);
        ItemSlot[currentSlotIndex].isSelected = true;
    }

    public GameObject GetPrefabForItem(string itemName)
    {
        foreach (var tool in allTools)
        {
            if (tool.Id == itemName)
                return tool.Prefab;
        }

        Debug.LogWarning($"No prefab assigned for item: {itemName}");
        return null;
    }
}

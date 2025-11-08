using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    public string itemName;
    public int quantity;
    public Sprite itemIcon;
    public bool isFull;

    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI quantityText;
    public GameObject selectedShader;
    public bool isSelected;

    private InventoryUIManager inventoryUIManager;

    private void Awake()
    {
        inventoryUIManager = GameObject.Find("Inventory").GetComponent<InventoryUIManager>();
    }

    public void AddItem(string itemName, int quantity, Sprite itemIcon)
    {
        this.itemName = itemName;
        this.quantity = quantity;
        this.itemIcon = itemIcon;
        isFull = true;
        iconImage.sprite = itemIcon;
        iconImage.enabled = true;
        quantityText.text = quantity.ToString();
        quantityText.enabled = true;
    }

    public void ClearSlot()
    {
        itemName = null;
        quantity = 0;
        itemIcon = null;
        isFull = false;
        iconImage.sprite = null;
        iconImage.enabled = false;
        quantityText.text = "";
        quantityText.enabled = false;

        if (selectedShader != null)
            selectedShader.SetActive(false);

        isSelected = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClickInput();
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClickInput();
        }
    }

    // Highlight slot
    public void OnRightClickInput()
    {
        inventoryUIManager.DeselectAllSlots();
        selectedShader.SetActive(true);
        isSelected = true;
    }

    public void OnLeftClickInput()
    {
        if (quantity <= 0 || !isFull) return;

        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Player not found!");
            return;
        }

        GameObject prefab = inventoryUIManager.GetPrefabForItem(itemName);
        if (prefab == null)
        {
            Debug.LogWarning($"No prefab assigned for item '{itemName}'");
            return;
        }

        for (int i = 0; i < quantity; i++)
        {
            Vector3 spawnPos = player.transform.position + player.transform.forward * 4f + new Vector3(i * 2f, 0, 0);
            GameObject droppedItem = Instantiate(prefab, spawnPos, Quaternion.identity);

            Tools tool = droppedItem.GetComponent<Tools>();
            if (tool != null)
            {
                tool.Drop();
            }
        }

        ClearSlot();
    }
}

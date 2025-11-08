using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private int quantity;
    [SerializeField] private Sprite itemIcon;

    [SerializeField] private InventoryUIManager inventoryUIManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Item pickup: {itemName}, quantity: {quantity}, sprite: {itemIcon?.name}");

            inventoryUIManager.AddItem(itemName, quantity, itemIcon);
            Destroy(transform.parent.gameObject); 
        }
    }
}

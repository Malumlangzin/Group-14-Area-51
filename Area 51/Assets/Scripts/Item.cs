using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private string itemName;

    [SerializeField]
    private int quantity;

    [SerializeField]    
    private Sprite itemIcon;

    private InventoryUIManager inventoryUIManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     // inventoryUIManager = GameObject.Find("Inventory").GetComponent<InventoryUIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}

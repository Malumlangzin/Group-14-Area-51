using UnityEngine;


public class Item : MonoBehaviour
{

    [SerializeField]
    private string itemName;

    [SerializeField]
    private string itemDescription;

    [SerializeField]    
    private Sprite sprite;

    private InventoryUIManager inventoryUIManager;


    void Start()
    {
        inventoryUIManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryUIManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            inventoryUIManager.AddItem(itemName, itemDescription , sprite);
            Destroy(gameObject);
        }
    }

 
}

using TMPro;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEditor.Progress;


public class InventoryData : MonoBehaviour
{
    public int ID;
    private Animator anim;
    public string itemName;
    public TextMeshProUGUI itemText;
    public Image selectedItem;
    private bool selected = false;
    public Sprite icon;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (selected)
        {
            selectedItem.sprite = icon;
            itemText.text = itemName;
        }
    }

    public void Selected()
    {
        selected = true;
        InventoryWheelController.inventoryID = ID;
    }

    public void Deselected()
    {
        selected = false;
        InventoryWheelController.inventoryID = 0;
    }

    public void HoverEnter()
    {
        anim.SetBool("Hover", false);
        itemText.text = itemName;
    }

    public void HoverEixt()
    {
        anim.SetBool("Hover", false);
        itemText.text = "";
    }
}

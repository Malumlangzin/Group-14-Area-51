using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private string inventoryId;
    private System.Action<string> dropAction;

    public int ID { get; private set; }
    public string ItemName { get; private set; }
    public Sprite Icon { get; private set; }

    [Header("UI refs")]
    [SerializeField] private Image iconImage;          
    [SerializeField] private TextMeshProUGUI nameText;   
    [SerializeField] private GameObject hoverHighlight; 

    public static event Action<int, Sprite, string> OnItemSelected;

    public void Initialize(string id, Item item, System.Action<string> dropAction)
    {
        this.inventoryId = id;
        this.dropAction = dropAction;
        ID = item.id;
        ItemName = item.itemName;
        Icon = item.icon;

        if (iconImage) iconImage.sprite = Icon;
        if (nameText) nameText.text = "";
        if (hoverHighlight) hoverHighlight.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnItemSelected?.Invoke(ID, Icon, ItemName);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverHighlight) hoverHighlight.SetActive(true);
        if (nameText) nameText.text = ItemName;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverHighlight) hoverHighlight.SetActive(false);
        if (nameText) nameText.text = "";
    }

    public void DropThisItem()
    {
        dropAction?.Invoke(inventoryId);
    }
}

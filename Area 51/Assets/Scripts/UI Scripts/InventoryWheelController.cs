using UnityEngine;
using UnityEngine.UI;

public class InventoryWheelController : MonoBehaviour
{
    public static InventoryWheelController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject wheelPanel; 
    [SerializeField] private Image selectedItemImage;
    [SerializeField] private TMPro.TextMeshProUGUI selectedItemText;
    [SerializeField] private Sprite noImage;

    private bool isOpen = false;
    private int currentItemID = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;

        if (wheelPanel != null) wheelPanel.SetActive(false);
    }

    public void OnInventoryToggle()
    {
        ToggleWheel();
    }

    public void ToggleWheel()
    {
        isOpen = !isOpen;
        if (wheelPanel != null) wheelPanel.SetActive(isOpen);
    }

    public void UpdateSelectedItem(int id, Sprite icon, string name)
    {
        currentItemID = id;
        if (selectedItemImage) selectedItemImage.sprite = icon != null ? icon : noImage;
        if (selectedItemText) selectedItemText.text = name ?? "";
    }

    private void Update()
    {
        if (isOpen && currentItemID == 0 && selectedItemImage != null) selectedItemImage.sprite = noImage;
    }
}

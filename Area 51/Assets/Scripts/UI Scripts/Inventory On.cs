using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryOn : MonoBehaviour
{
    public Button InventoryButton;
    public GameObject inventoryWheel;

    private void Start()
    {
        InventoryButton.onClick.AddListener(ResumeGame);

        EventSystem.current.SetSelectedGameObject(InventoryButton.gameObject);

        inventoryWheel.SetActive(false);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inventoryWheel.activeSelf) ResumeGame();
        }

        if (inventoryWheel.activeSelf && EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(InventoryButton.gameObject);

        if (inventoryWheel.activeSelf && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            Button selectedButton = EventSystem.current.currentSelectedGameObject?.GetComponent<Button>();
            if (selectedButton != null)
                selectedButton.onClick.Invoke();

            Time.timeScale = 0f;
        }
    }

    public void ResumeGame()
    {
        inventoryWheel.SetActive(false);
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryWheelController : MonoBehaviour
{
    public Animator anim;
    private bool InventoryWheelSelected = false;
    public Image selectedItem;
    public Sprite noImage;
    public static int inventoryID;

    public void OnInventoryOpen(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InventoryWheelSelected = !InventoryWheelSelected;
        }
    }

    void Update()
    {


        if (InventoryWheelSelected)
        {
            anim.SetBool("OpenInventoryWheel", true);
        }
        else
        {
            anim.SetBool("OpenInventoryWheel", false);
        }

        switch (inventoryID)
        {
            case 0:
                selectedItem.sprite = noImage;
                break;
            case 1:
                print("Modulator");
                break;
            case 2:
                print("Astromech socket");
                break;
            case 3:
                print("Astromech socket");
                break;
        }
    }
}

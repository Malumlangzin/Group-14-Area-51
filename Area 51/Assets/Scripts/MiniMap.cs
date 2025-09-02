using UnityEngine;
using UnityEngine.InputSystem;

public class MiniMap : MonoBehaviour
{
    public GameObject MiniMapUI;

    public void OnMiniMap(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
          MiniMapUI.SetActive(true);
        }
        else if (context.canceled)
        {
            MiniMapUI.SetActive(false);
        }
       
    }
}

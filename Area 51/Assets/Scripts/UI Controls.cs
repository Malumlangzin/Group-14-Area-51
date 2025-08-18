using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

    public void OnStart(InputAction.CallbackContext context)
    {
        print("any key pressed");
        SceneManager.LoadSceneAsync(1);
    }
}
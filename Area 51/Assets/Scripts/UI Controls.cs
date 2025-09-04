using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject PauseUi;
    
    public void OnStart(InputAction.CallbackContext context)
    {
        print("any key pressed");
        SceneManager.LoadSceneAsync(1);
    }
   
    public void OnPause()
    {
        PauseUi.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }
 public void Quitgame()
    {
        print("Quitgame");
        Application.Quit();
    }

    public void OnPlayPress()
    {
        PauseUi.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1.0f;
    }

}
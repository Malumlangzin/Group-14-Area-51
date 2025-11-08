using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class UIControls : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject Options; 
    public GameObject PauseUi;
    public GameObject Volume;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }


    public void Resume()
    {
        PauseUi.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        PauseUi.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
    }
     public void OnOptionsPress()
     {
         Options.SetActive(true);
         PauseUi.SetActive(false);
         Time.timeScale = 0f;
     } 
    
    public void OnVolume()
    {
         Volume.SetActive(true);
         Cursor.visible = true;

         Time.timeScale = 0f;

         Cursor.lockState = CursorLockMode.None;
         Cursor.visible = true;
    }
     public void OnBackPress()
     {
         Options.SetActive(false);
         PauseUi.SetActive(true);
         Time.timeScale = 0f;
     }
 
     public void OnStart(InputAction.CallbackContext context)
     {
         print("any key pressed");
         SceneManager.LoadSceneAsync(2);
     }
    /*
     public void OnPause()
     {
         PauseUi.SetActive(true);

         Cursor.visible = true;

         Time.timeScale = 0f;

         Cursor.lockState = CursorLockMode.None;
         Cursor.visible = true;

     }

     public void OnVolBack()
     {
         Volume.SetActive(false);
         Time.timeScale = 1.0f;
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
     }*/

}
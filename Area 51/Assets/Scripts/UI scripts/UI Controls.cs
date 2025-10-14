using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class UIControls : MonoBehaviour
{
    public GameObject PauseUi;
    public GameObject Options;
    public GameObject Volume;
    public void OnStart(InputAction.CallbackContext context)
    {
        print("any key pressed");
        SceneManager.LoadSceneAsync(1);
    }
   
    public void OnPause()
    {
        PauseUi.SetActive(true);

        Cursor.visible = true;

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public void OnVolume()
    {
        Volume.SetActive(true);

        Cursor.visible = true;

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void OnOptionsPress()
    {
        Options.SetActive(true);
        PauseUi.SetActive(false);
        Time.timeScale = 0f;
    }

    public void OnVolBack()
    {
        Volume.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void OnBackPress()
    {
        Options.SetActive(false);
        PauseUi.SetActive(true);
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
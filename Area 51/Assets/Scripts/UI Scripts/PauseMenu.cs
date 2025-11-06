using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
public class PauseMenu : MonoBehaviour
{

    private Controls Controls;
    private InputAction menu;

    [SerializeField] private GameObject Pausemenu;
    [SerializeField] private bool isPaused;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Controls = new Controls(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        menu = Controls.Menu.Escape;
        menu.Enable();

        menu.performed += Pause;
    }

    private void OnDisable()
    {
        menu.Disable();
    }

    void Pause(InputAction.CallbackContext context)
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            ActivateMenu();
        }
        else
        {
            DeactivateMenu();
        }
    }

    void ActivateMenu()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        Pausemenu.SetActive(true);
    }

    public void DeactivateMenu()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        Pausemenu.SetActive(false);
        isPaused = false;
    }


    public void QuitGame()
    {
        Debug.Log("Quitting game...");
    }

}

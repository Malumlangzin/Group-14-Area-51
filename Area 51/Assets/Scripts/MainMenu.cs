using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{
    public Button playButton;
    public Button quitButton;
    public GameObject pauseMenuUI;

    private void Start()
    {
        playButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitGame);
        EventSystem.current.SetSelectedGameObject(playButton.gameObject);
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(playButton.gameObject);

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Button selectedButton = EventSystem.current.currentSelectedGameObject?.GetComponent<Button>();
            if (selectedButton != null)
                selectedButton.onClick.Invoke();
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        UnityEngine.InputSystem.EnhancedTouch.EnhancedTouchSupport.Enable();
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class WinLogic : MonoBehaviour
{
    [Header("Required Item IDs")]
    [SerializeField] private string[] requiredItemIDs = { "Modulator", "Hyperdrive", "Astromech Socket" };

    [Header("Animation Settings")]
    [SerializeField] private Animator shipAnimator;
    [SerializeField] private string winTriggerName = "OnWin";
    [SerializeField] private float exitDelay = 2f;

    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerBody;
    [SerializeField] private MonoBehaviour[] disableOnWin;
    public GameObject WinUi;

    private int itemsInside = 0;
    private bool hasWon = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasWon) return;
        Tools tool = other.GetComponent<Tools>();
        if (tool == null) return;

        foreach (string id in requiredItemIDs)
        {
            if (tool.Id == id)
            {
                itemsInside++;
                Debug.Log($"✅ Item '{tool.Id}' entered the ship. ({itemsInside}/{requiredItemIDs.Length})");

                if (itemsInside >= requiredItemIDs.Length)
                    StartCoroutine(HandleWinSequence());

                break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (hasWon) return;
        Tools tool = other.GetComponent<Tools>();
        if (tool == null) return;

        foreach (string id in requiredItemIDs)
        {
            if (tool.Id == id)
            {
                itemsInside--;
                Debug.Log($"❌ Item '{tool.Id}' left the ship. ({itemsInside}/{requiredItemIDs.Length})");
                break;
            }
        }
    }

    private IEnumerator HandleWinSequence()
    {
        hasWon = true;
        Debug.Log("🎉 All items collected! YOU WIN!");

        foreach (var script in disableOnWin)
        {
            if (script != null)
                script.enabled = false;
        }

        if (playerBody != null)
            Destroy(playerBody);

        if (shipAnimator != null)
        {
            shipAnimator.SetTrigger(winTriggerName);
            yield return new WaitForSeconds(GetAnimationClipLength(shipAnimator, winTriggerName) + exitDelay);
        }
        else
        {
            yield return new WaitForSeconds(exitDelay);
        }

        WinUi.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;



#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private float GetAnimationClipLength(Animator animator, string triggerName)
    {
        if (animator.runtimeAnimatorController == null) return 0f;
        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == triggerName || clip.name.ToLower().Contains("win"))
                return clip.length;
        }
        return 2f;
    }

    public void OnStart()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting game...");
    }
}

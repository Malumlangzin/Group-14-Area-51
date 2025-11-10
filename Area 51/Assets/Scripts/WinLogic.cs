using UnityEngine;
using System.Collections;

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

    private int itemsInside = 0;
    private bool hasWon = false;

    private void OnTriggerEnter(Collider other)
    {
        Tools tool = other.GetComponent<Tools>();
        if (tool == null || hasWon) return;

        foreach (string id in requiredItemIDs)
        {
            if (tool.Id == id)
            {
                itemsInside++;
                Debug.Log($"✅ Item '{tool.Id}' entered the ship. ({itemsInside}/{requiredItemIDs.Length})");

                if (itemsInside >= requiredItemIDs.Length)
                {
                    StartCoroutine(HandleWinSequence());
                }

                break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Tools tool = other.GetComponent<Tools>();
        if (tool == null || hasWon) return;

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

      
        if (player != null)
        {
            Destroy(player);
        }

        
        if (shipAnimator != null)
        {
            shipAnimator.SetTrigger(winTriggerName);

           
            yield return new WaitForSeconds(GetAnimationClipLength(shipAnimator, winTriggerName) + exitDelay);
        }
        else
        {
            yield return new WaitForSeconds(exitDelay);
        }

       
        Application.Quit();

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
}

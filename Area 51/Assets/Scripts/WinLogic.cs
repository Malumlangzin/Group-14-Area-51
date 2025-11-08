using UnityEngine;

public class WinLogic : MonoBehaviour
{
    [Header("Required Item IDs")]
    [SerializeField] private string[] requiredItemIDs = { "Modulator", "Hyperdrive", "Astromech Socket" };

    [Header("Animation Settings")]
    [SerializeField] private Animator shipAnimator;
    [SerializeField] private string winTriggerName = "OnWin";

    private int itemsInside = 0;

    private void OnTriggerEnter(Collider other)
    {
        Tools tool = other.GetComponent<Tools>();
        if (tool == null) return;

        foreach (string id in requiredItemIDs)
        {
            if (tool.Id == id)
            {
                itemsInside++;
                Debug.Log($"✅ Item '{tool.Id}' entered the ship. ({itemsInside}/{requiredItemIDs.Length})");

                if (itemsInside >= requiredItemIDs.Length)
                {
                    TriggerWin();
                }

                break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
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

    private void TriggerWin()
    {
        Debug.Log("🎉 All items collected! YOU WIN!");
        if (shipAnimator != null)
        {
            shipAnimator.SetTrigger(winTriggerName);
        }
    }
}

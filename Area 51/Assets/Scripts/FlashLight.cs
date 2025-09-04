using UnityEngine;
using System.Collections;

public class FlashLight : MonoBehaviour
{
    public Light warningLight;          // Drag your HDRP Light here
    public float blinkInterval = 0.5f;  // Seconds between toggles

    void Start()
    {
        if (warningLight == null)
            warningLight = GetComponent<Light>();

        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {
            warningLight.enabled = !warningLight.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}

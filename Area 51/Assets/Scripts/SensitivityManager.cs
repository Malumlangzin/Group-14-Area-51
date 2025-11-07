using UnityEngine;
using UnityEngine.UI;

public class SensitivityManager : MonoBehaviour
{
    public static float value;
    public Slider slider;
    void Start()
    {
        value = PlayerPrefs.GetFloat("sens", 1f);
        slider.value = value;
        slider.onValueChanged.AddListener(v => { value = v; PlayerPrefs.SetFloat("sens", v); });
    }
}

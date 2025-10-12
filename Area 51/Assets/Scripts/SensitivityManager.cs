using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Canvas))]
public class SensitivityManager : MonoBehaviour
{
    public static float value;
    public Slider slider;

    private void Start()
    {
        value = PlayerPrefs.GetFloat("sens", 1f);
        slider.value = value;

        slider.onValueChanged.AddListener(v =>
        {
            value = v;
            PlayerPrefs.SetFloat("sens", v);
        });
    }
}


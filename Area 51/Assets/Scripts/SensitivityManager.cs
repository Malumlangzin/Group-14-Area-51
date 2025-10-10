using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Canvas))]
public class SensitivityManager : MonoBehaviour
{
    public InputActionAsset inputActions;
    public string actionMapName = "Player";
    public Slider sensitivitySlider;
    public float defaultSensitivity = 2f;
    public string playerPrefsKey = "sensitivity";

    void Start()
    {
        if (sensitivitySlider == null || inputActions == null) return;

        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);

        float saved = PlayerPrefs.GetFloat(playerPrefsKey, defaultSensitivity);
        saved = Mathf.Clamp(saved, sensitivitySlider.minValue, sensitivitySlider.maxValue);
        sensitivitySlider.value = saved;

        ApplySensitivityToBindings(saved);
    }

    void OnDestroy()
    {
        if (sensitivitySlider != null)
            sensitivitySlider.onValueChanged.RemoveListener(SetSensitivity);
    }

    public void SetSensitivity(float sliderValue)
    {
        float sens = Mathf.Max(0.0001f, sliderValue);
        PlayerPrefs.SetFloat(playerPrefsKey, sens);
        PlayerPrefs.Save();
        ApplySensitivityToBindings(sens);
    }

    private void ApplySensitivityToBindings(float multiplier)
    {
        var map = inputActions.FindActionMap(actionMapName, false);
        if (map == null) return;

        foreach (var action in map.actions)
        {
            var bindings = action.bindings;
            for (int b = 0; b < bindings.Count; ++b)
            {
                var binding = bindings[b];
                if (binding.isPartOfComposite) continue;

                string path = binding.path ?? "";
                string lowerPath = path.ToLowerInvariant();
                string processorsOverride = null;

                bool looksLikeMouseDelta = lowerPath.Contains("mouse") && lowerPath.Contains("delta");
                bool looksLikeStick = lowerPath.Contains("stick") || lowerPath.Contains("rightstick") || lowerPath.Contains("leftstick") || lowerPath.Contains("thumbstick");
                bool looksLikeVector2 = looksLikeMouseDelta || looksLikeStick || lowerPath.Contains("dpad") || lowerPath.Contains("vector2");
                bool looksLikeFloat = lowerPath.Contains("trigger") || lowerPath.Contains("axis") || lowerPath.Contains("pressure") || (lowerPath.Contains("delta") && !lowerPath.Contains("mouse"));

                if (looksLikeVector2)
                    processorsOverride = $"scaleVector2(x={multiplier:F3},y={multiplier:F3})";
                else if (looksLikeFloat)
                    processorsOverride = $"scale(factor={multiplier:F3})";
                else if (string.Equals(action.expectedControlType, "Vector2", StringComparison.OrdinalIgnoreCase))
                    processorsOverride = $"scaleVector2(x={multiplier:F3},y={multiplier:F3})";
                else if (string.Equals(action.expectedControlType, "Axis", StringComparison.OrdinalIgnoreCase))
                    processorsOverride = $"scale(factor={multiplier:F3})";

                if (!string.IsNullOrEmpty(processorsOverride))
                {
                    try
                    {
                        action.ApplyBindingOverride(b, new InputBinding { overrideProcessors = processorsOverride });
                    }
                    catch (Exception) { }
                }
            }
        }

        Debug.Log($"Applied sensitivity x{multiplier:F2}");
    }

    public void ClearAllBindingOverrides()
    {
        var map = inputActions.FindActionMap(actionMapName, false);
        if (map == null) return;
        foreach (var action in map.actions)
            action.RemoveAllBindingOverrides();
    }
}


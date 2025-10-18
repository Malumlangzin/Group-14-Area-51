using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20);
    }


    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("Music", Mathf.Log10(level) * 20);
    }


    public void SetSoundFXVolume(float level)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(level) * 20);
    }
}

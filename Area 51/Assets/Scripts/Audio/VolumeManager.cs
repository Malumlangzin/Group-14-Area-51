using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider MasterSlider;

    public void SetMasterVolume(float level)
    {
        float voulme = MasterSlider.value;
       
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20);
    }


    public void SetMusicVolume(float level)
    { 
        float voulme = musicSlider.value;
       
        audioMixer.SetFloat("Music", Mathf.Log10(level) * 20);
    }


    public void SetSoundFXVolume(float level)
    { 
        float voulme = musicSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(level) * 20);
    }
}

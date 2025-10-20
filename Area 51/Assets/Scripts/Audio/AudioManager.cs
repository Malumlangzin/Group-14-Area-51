using UnityEditor.Timeline;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioManager audioManager;

    [Header("--Audio Source-- ")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;


    [Header("--Audio Clip--")]
    public AudioClip background;
    public AudioClip pickup;
    public AudioClip Drop;
    
   
    private void Start()
    {
       musicSource.clip = background;
       musicSource.Play();
    }
   
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}

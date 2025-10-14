using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioManager audioManager;
    public GameObject PauseUi;

    [Header("--Audio Source-- ")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource IntroSound;
    [SerializeField] AudioSource SFXSource;

    [Header("--Audio Clip--")]
    public AudioClip background;
    public AudioClip Intro;
    public AudioClip pickUp;
    public AudioClip Drop;
    
   
    private void Start()
    {
       musicSource.clip = background;
       musicSource.Play();
    }

    public void OnplayPress()
    {
        PauseUi.SetActive(true);
       //udioManager.PlaySFX(audioManager.buttonClick);
        Time.timeScale = 1.0f;
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}

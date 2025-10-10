using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioManager audioManager;
    public GameObject PauseUi;

    [Header("--Audio Source-- ")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("--Audio Clip--")]
    public AudioClip background;
    public AudioClip macestrike;
    public AudioClip buttonClick;
    public AudioClip swordStrike;
    
   
    private void Start()
    {
       musicSource.clip = background;
       musicSource.Play();
    }

    public void OnplayPress()
    {
        PauseUi.SetActive(true);
        audioManager.PlaySFX(audioManager.buttonClick);
        Time.timeScale = 1.0f;
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}

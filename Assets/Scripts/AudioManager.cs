using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Source")]
    public AudioClip throwSound;
    public AudioClip backgroundMusic;
    public AudioClip jump;
    public AudioClip attack;
    public AudioClip run;
    public AudioClip walk;
    
    private void Start(){
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip){
        SFXSource.PlayOneShot(clip);
    }

    public void StopSFX(){
        SFXSource.Stop();
    }
}

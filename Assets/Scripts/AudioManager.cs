using UnityEngine;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource movementSFXSource;
    [SerializeField] private AudioSource actionSFXSource;

    [Header("Audio Clips")]
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
        if (clip == walk || clip == run) 
        {
            PlayMovementSFX(clip);
        }
        else 
        {
            PlayActionSFX(clip);
        }
    }

    public void StopSFX(){
        movementSFXSource.Stop();
    }

    private void PlayMovementSFX(AudioClip clip)
    {
        if (clip != null && movementSFXSource != null && !movementSFXSource.isPlaying)
        {
            movementSFXSource.clip = clip;
            movementSFXSource.Play();
        }
    }

    private void StopMovementSFX()
    {
        if (movementSFXSource != null)
        {
            movementSFXSource.Stop();
        }
    }

    private void PlayActionSFX(AudioClip clip)
    {
        if (clip != null && actionSFXSource != null)
        {
            actionSFXSource.PlayOneShot(clip); 
        }
    }
}

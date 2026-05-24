using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource vfxAudioSource;
    public AudioClip musicClip;
    void Start()
    {
        audioSource.clip = musicClip;
        audioSource.Play();
    }
}

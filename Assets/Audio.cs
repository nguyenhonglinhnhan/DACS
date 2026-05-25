using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip musicClip;
    
    void Awake()
    {
        // Chỉ tạo 1 instance duy nhất
        if (FindObjectsOfType<Audio>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject); // Giữ object này qua các scene
        
        audioSource = GetComponent<AudioSource>();
        
        if (audioSource != null && musicClip != null)
        {
            audioSource.clip = musicClip;
            audioSource.Play();
        }
    }
}
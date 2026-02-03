using UnityEngine;
using UnityEngine.InputSystem; // Quan trọng

public class PlaySoundOnE : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sound;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Kiểm tra phím E bằng New Input System
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            PlaySound();
        }
    }

    void PlaySound()
    {
        if (sound != null && audioSource != null)
        {
            audioSource.PlayOneShot(sound);
        }
    }
}

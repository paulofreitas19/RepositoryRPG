using UnityEngine;
using System;
using SysDiag = System.Diagnostics; // <- alias

public class ActorSFX : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        if (!audioSource.isPlaying) // <-- só toca se não estiver tocando nada
            audioSource.PlayOneShot(clip);
    }
}

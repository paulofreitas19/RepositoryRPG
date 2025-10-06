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

        if (!audioSource.isPlaying) // <-- s� toca se n�o estiver tocando nada
            audioSource.PlayOneShot(clip);
    }
}

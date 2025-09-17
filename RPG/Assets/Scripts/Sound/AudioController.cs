using System.Collections;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    [Header("Global Audio Source (BGM)")]
    [SerializeField] private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Toca música de fundo global.
    /// </summary>
    public void PlayBGM(AudioClip audio, bool loop = true, float volume = 0.2f)
    {
        if (audioSource == null) return;

        audioSource.clip = audio;
        audioSource.loop = loop;
        audioSource.volume = volume;
        audioSource.Play();
    }

    /// <summary>
    /// Toca um som e destrói o objeto alvo quando o som acabar.
    /// </summary>
    public void PlayAndDestroy(AudioClip clip, Vector3 position, GameObject target)
    {
        if (clip == null) return;
        StartCoroutine(PlayAndDestroyRoutine(clip, position, target));
    }

    private IEnumerator PlayAndDestroyRoutine(AudioClip clip, Vector3 position, GameObject target)
    {
        // Cria objeto temporário só para o som
        GameObject temp = new GameObject("TempAudio");
        AudioSource source = temp.AddComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        temp.transform.position = position;

        // Espera terminar o áudio
        yield return new WaitForSeconds(clip.length);

        if (target != null)
            Destroy(target);

        Destroy(temp);
    }
}

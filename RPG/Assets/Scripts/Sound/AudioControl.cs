using UnityEngine;

public class AudioControl : MonoBehaviour
{
    [SerializeField] private AudioClip bgmMusic;

    private AudioController audioController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioController = FindObjectOfType<AudioController>();

        audioController.PlayBGM(bgmMusic);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private AudioController soundController;
    [SerializeField] private AudioClip pickCoinAudio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        soundController = FindObjectOfType<AudioController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            //SoundController.instance.PlaySound(GetComponent<AudioSource>());
            coll.GetComponent<PlayerItems>().CurrentGold++;
            AudioController.instance.DestroyAfterSound();
        }
    }
}

using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private AudioSource audio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            audio.Play();
            coll.GetComponent<PlayerItems>().CurrentGold++;
            StartCoroutine(DestroyAfterSound());
        }
    }

    IEnumerator DestroyAfterSound()
    {
        yield return new WaitForSeconds(audio.clip.length);
        Destroy(gameObject);
    }

}

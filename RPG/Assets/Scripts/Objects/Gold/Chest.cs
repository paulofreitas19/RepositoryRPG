using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private PlayerItems playerItems;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    [SerializeField] private Sprite openChest;
    [SerializeField] private AudioClip pickChestAudio;
    [SerializeField] private AudioClip openChestAudio;

    private bool detecting;
    private bool isOpened;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        playerItems = FindObjectOfType<PlayerItems>();
    }

    void Update()
    {
        OnGet();
    }

    void OnGet()
    {
        if (Input.GetKeyDown(KeyCode.E) && detecting)
        {
            if (!isOpened)
            {
                // Abrir o baú
                audioSource.PlayOneShot(openChestAudio);
                spriteRenderer.sprite = openChest;
                isOpened = true;
            }
            else
            {
                // Coletar
                playerItems.CurrentGold += 10;
                AudioController.instance.PlayAndDestroy(pickChestAudio, transform.position, null);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
            detecting = true;
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
            detecting = false;
    }
}

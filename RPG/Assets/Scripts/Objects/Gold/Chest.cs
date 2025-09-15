using UnityEngine;

public class Chest : MonoBehaviour
{
    private PlayerItems playerItems;

    private SpriteRenderer spriteRenderer;

    [SerializeField]private Sprite openChest;

    private bool detecting;
    private bool pressButton;
    private bool isOpened;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerItems = FindObjectOfType<PlayerItems>();
    }

    // Update is called once per frame
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
                spriteRenderer.sprite = openChest;

                isOpened = true;
            }

            else
            {
                playerItems.CurrentGold+=10;
                Destroy(gameObject);
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            detecting = true;
        }
    }

}

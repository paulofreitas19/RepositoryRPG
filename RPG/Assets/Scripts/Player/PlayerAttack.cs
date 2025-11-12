using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {

            if (collision.GetComponent<Spider>())
            {
                collision.GetComponent<Spider>().OnHit(0.5f); 
            }

            if (collision.GetComponent<Dino>())
            {
                collision.GetComponent<Dino>().OnHit(0.5f); 
            }

            
        }
    }
}

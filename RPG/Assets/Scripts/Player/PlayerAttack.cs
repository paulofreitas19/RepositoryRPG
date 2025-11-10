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
            var player = GetComponentInParent<Player>();
            player.isStomping = true;

            collision.GetComponent<Dino>().OnHit(); // ou aplicar dano

        }
    }
}

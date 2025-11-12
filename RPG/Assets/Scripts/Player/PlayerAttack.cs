using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Player player;
    [SerializeField] private float bounceForce = 10f;

    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // O player pula na cabeça do inimigo — aplica dano no inimigo
            if (collision.GetComponent<Spider>())
                collision.GetComponent<Spider>().OnHit(0.5f);

            if (collision.GetComponent<Dino>())
                collision.GetComponent<Dino>().OnHit(0.5f);

            // Impulso de "rebote" para cima
            Rigidbody2D rig = player.GetComponent<Rigidbody2D>();
            rig.linearVelocity = new Vector2(rig.linearVelocity.x, bounceForce);

            // Protege o player temporariamente para não tomar dano do inimigo
            player.StartCoroutine(player.TemporaryInvulnerability(0.3f));
        }
    }
}

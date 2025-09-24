using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Player player;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private float blinkTimer;
    [SerializeField] private float blinkInterval = 0.2f;

    void Start()
    {
        player = GetComponent<Player>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        HandleAnimations();
        HandleInvulnerabilityBlink();
    }

    void HandleAnimations()
    {
        if (player.IsDeath)
            anim.SetTrigger("isDeath");
        else if (player.IsHit)
            anim.SetTrigger("isHit");
        else if (player.IsAttacking)
            anim.SetInteger("transition", 2);
        else if (player.IsClimbing)
            anim.SetInteger("transition", 3);
        else if (player.IsJumping)
            anim.SetInteger("transition", 4);
        else if (player.IsRunning)
            anim.SetInteger("transition", 1);
        else
            anim.SetInteger("transition", 0);
    }

    void HandleInvulnerabilityBlink()
    {
        if (!player.IsInvulnerable)
        {
            spriteRenderer.enabled = true;
            return;
        }

        blinkTimer += Time.deltaTime;
        if (blinkTimer >= blinkInterval)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            blinkTimer = 0;
        }
    }
}
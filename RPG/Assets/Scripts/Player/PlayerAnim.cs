using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Player player;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private float blinkTimer;
    [SerializeField] private float blinkInterval = 0.2f;

    private bool hasPlayedDeathAnim;

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
        {
            if (!hasPlayedDeathAnim)
            {
                anim.SetTrigger("isDeath");
                hasPlayedDeathAnim = true;
            }
            return; // impede que outras animações sobrescrevam
        }

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

        hasPlayedDeathAnim = false; // reseta quando sair da morte
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
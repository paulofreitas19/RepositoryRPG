using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rig;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private AudioClip landingSound;

    [Header("Stats")]
    [SerializeField] private int currentGold = 0;
    [SerializeField] private float health;
    [SerializeField] private float maxHealth = 1f;
    [SerializeField] private int lifePoints = 3;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float climbSpeed = 3f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float radius = 0.5f;
    [SerializeField] private float attackAnimDuration = 0.4f;
    [SerializeField] private float attackCooldown = 1f;

    [Header("Combat")]
    [SerializeField] private Transform pointAttack;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask platformLayer;

    // Estado interno
    private float movement;
    private float vertical;

    private bool isJumping;
    private bool isRunning;
    private bool isAttacking;
    private bool canAttack = true;
    private bool canClimb;
    private bool isClimbing;
    private bool isHit;
    private bool isDeath;
    private bool canTakeHit = true;
    private bool isInvulnerable;
    private bool isStunned;

    #region Propriedades públicas (acessadas pelo PlayerAnim)
    public int CurrentGold { get => currentGold; set => currentGold = value; }
    public int LifePoints { get => lifePoints; set => lifePoints = value; }
    public float Health { get => health; set => health = value; }
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }

    public bool IsJumping { get => isJumping; }
    public bool IsRunning { get => isRunning; }
    public bool IsAttacking { get => isAttacking; }
    public bool IsClimbing { get => isClimbing; }
    public bool IsHit { get => isHit; }
    public bool IsDeath { get => isDeath; }
    public bool IsInvulnerable { get => isInvulnerable; }
    #endregion

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        health = maxHealth;
    }

    void Update()
    {
        OnJump();
        OnAttack();
        OnClimb();
        OnDropPlatform();
    }

    void FixedUpdate()
    {
        OnMove();
    }

    #region Movimentação
    void OnMove()
    {
        if (isStunned)
        {
            rig.linearVelocity = Vector2.zero;
            return;
        }

        if (isAttacking) return;

        movement = Input.GetAxis("Horizontal");
        rig.linearVelocity = new Vector2(movement * speed, rig.linearVelocity.y);

        if (movement == 0 && !isJumping && !isClimbing)
        {
            isRunning = false;
        }
        else if (movement != 0)
        {
            isRunning = true;
            transform.eulerAngles = (movement < 0) ? new Vector2(0, 180) : new Vector2(0, 0);
        }
    }

    void OnJump()
    {
        if (!isJumping && Input.GetKeyDown(KeyCode.Space))
        {
            rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isJumping = true;
        }
    }

    void OnClimb()
    {
        if (canClimb)
        {
            vertical = Input.GetAxis("Vertical");
            if (vertical != 0)
            {
                rig.gravityScale = 0f;
                rig.linearVelocity = new Vector2(rig.linearVelocity.x, vertical * climbSpeed);
                isClimbing = true;
            }
            else
            {
                rig.linearVelocity = new Vector2(rig.linearVelocity.x, 0);
                isClimbing = false;
            }
        }
        else
        {
            rig.gravityScale = 3.5f;
            isClimbing = false;
        }
    }

    void OnDropPlatform()
    {
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !isJumping && canClimb)
        {
            var platform = GetPlatformBelow();
            if (platform != null)
                StartCoroutine(DropThrough(platform));
        }
    }
    #endregion

    #region Combate
    void OnAttack()
    {
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            Collider2D hit = Physics2D.OverlapCircle(pointAttack.position, radius, enemyLayer);

            if (hit != null)
            {
                if (hit.GetComponent<Spider>())
                {
                    hit.transform.GetComponent<Spider>().OnHit(0.5f);
                }
            }

            isAttacking = true;
            canAttack = false;

            StartCoroutine(AttackRoutine());
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(pointAttack.position, radius);   
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(attackAnimDuration);
        isAttacking = false;

        float remaining = attackCooldown - attackAnimDuration;
        if (remaining > 0f)
            yield return new WaitForSeconds(remaining);

        canAttack = true;
    }
    #endregion

    #region Dano e Vida
    public void OnHit(float damage)
    {
        if (!canTakeHit) return;

        canTakeHit = false;

        isHit = true;
        health -= damage;

        if (health <= 0.01f)
        {
            StartCoroutine(DeathSequence());
            return; // <- importante
        }

        //StartCoroutine(StunTime());
        StartCoroutine(RecoveryTime());
    }


    //private IEnumerator StunTime()
    //{
    //    isInvulnerable = true;
    //    isStunned = true;
    //    yield return new WaitForSeconds(1.5f);
    //    isStunned = false;
    //    isHit = false;   
    //}

    private IEnumerator RecoveryTime()
    {
        canTakeHit = false;
        isInvulnerable = true;
        yield return new WaitForSeconds(4f);
        canTakeHit = true;
        isInvulnerable = false;
        isHit = false;
    }

    public IEnumerator DeathSequence()
    {
        // Desativa controles
        isDeath = true;
        isStunned = true;
        isAttacking = false;
        rig.linearVelocity = Vector2.zero;

        // Aguarda ANIMAÇÃO acabar (confirme o tempo certo no Animator)
        yield return new WaitForSeconds(1f);

        // === RESPAWN ===
        PlayerPos.instance.CheckPoint();
        lifePoints--;
        health = maxHealth;

        // Aguarda 1 frame para garantir que posição foi atualizada
        yield return null;

        // Reseta estados
        isDeath = false;
        isHit = false;
        canTakeHit = true;
        isInvulnerable = false;

    }
    #endregion

    #region Plataformas
    Collider2D GetPlatformBelow()
    {
        var b = playerCollider.bounds;
        Vector2 origin = new Vector2(b.center.x, b.min.y - 0.02f);
        Vector2 size = new Vector2(b.size.x * 0.9f, 0.06f);
        return Physics2D.OverlapBox(origin, size, 0f, platformLayer);
    }

    IEnumerator DropThrough(Collider2D platform)
    {
        isClimbing = false;
        rig.gravityScale = 3.5f;
        Physics2D.IgnoreCollision(playerCollider, platform, true);
        rig.linearVelocity = new Vector2(rig.linearVelocity.x, -5f);

        for (int i = 0; i < 16; i++)
            yield return new WaitForFixedUpdate();

        Physics2D.IgnoreCollision(playerCollider, platform, false);
    }
    #endregion

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 8)
        {
            isJumping = false;
            AudioController.instance.PlayAndDestroy(landingSound, transform.position, null);
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Climb")) canClimb = true;

        if (coll.gameObject.layer == 7) OnHit(0.2f);
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Climb")) canClimb = false;
    }
}
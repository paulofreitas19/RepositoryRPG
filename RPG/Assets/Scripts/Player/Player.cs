using System.Collections;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Player : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rig;

    [Header("Stats")]
    [SerializeField] private int health;
    [SerializeField] private float speed;
    private float initialSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float climbSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float radius;
    [SerializeField] private float attackAnimDuration;
    [SerializeField] private float attackCooldown;
    private float movement;
    private float vertical;


    [SerializeField] Transform pointAttack;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] LayerMask platformLayer;
    [SerializeField] private Collider2D playerCollider;

    private bool isMoving;
    private bool isJumping;
    private bool doubleJumping;
    private bool isRunning;
    private bool isAttacking;
    private bool canAttack;
    private bool canClimb;
    private bool isClimbing;

    public bool IsMoving
    {
        get { return isMoving; }
        set { isMoving = value; }
    }

    public bool IsJumping
    {
        get { return isJumping; }
        set { isJumping = value; }
    }

    public bool IsRunning
    {
        get { return isRunning; }
        set { isRunning = value; }
    }

    public bool IsAttacking
    {
        get { return isAttacking; }
        set { isAttacking = value; }
    }

    public bool IsClimbing
    {
        get { return isClimbing; }
        set { isClimbing = value; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        initialSpeed = speed;
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        OnJump();
        OnRun();
        OnAttack();
        OnClimb();
        OnDropPlatform();
    }

    void FixedUpdate()
    {
        OnMove();
    }

    void OnMove()
    {
        if(isAttacking) return;

        movement = Input.GetAxis("Horizontal");
        rig.linearVelocity = new Vector2(movement * speed, rig.linearVelocity.y);

        if (movement == 0 && !isJumping && !isClimbing)
        {
            isMoving = false;
        }

        if (movement < 0)
        {
            isMoving = true;
            transform.eulerAngles = new Vector2(0, 180);
        }

        if(movement > 0)
        {
            isMoving = true;
            transform.eulerAngles = new Vector2(0, 0);
        }
    }

    void OnJump()
    {
        if (!isJumping)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {

                rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

                isJumping = true;
            }
        }
    }

    void OnRun()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isJumping && movement != 0)
        {
            speed = runSpeed;
            isRunning = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = initialSpeed;
            isRunning = false;
        }
    }

    void OnClimb()
    {
        if (canClimb)
        {
            vertical = Input.GetAxis("Vertical");

            if (vertical != 0)
            {
                rig.gravityScale = 0f; // Desliga a gravidade
                rig.linearVelocity = new Vector2(rig.linearVelocity.x, vertical * climbSpeed);
                isClimbing = true;
            }

            else
            {
                // Para se o player não estiver pressionando nada
                rig.linearVelocity = new Vector2(rig.linearVelocity.x, 0);
                isClimbing = false;
            }
        }
        else
        {
            // Restaura gravidade quando sai da escada
            rig.gravityScale = 3.5f;
            isClimbing = false;
            
        }
    }

    // Método chamado no Update, verifica se o jogador quer descer
    void OnDropPlatform()
    {
        // Se pressionar S ou seta para baixo, e não estiver pulando
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !isJumping)
        {
            // Verifica se existe uma plataforma logo abaixo dos pés
            var platform = GetPlatformBelow();

            // Se encontrou, inicia a corrotina de atravessar
            if (platform != null)
                StartCoroutine(DropThrough(platform));
        }
    }

    // Detecta a plataforma que está logo abaixo do Player
    Collider2D GetPlatformBelow()
    {
        // Pega os limites (bounding box) do collider do Player em coordenadas de mundo
        var b = playerCollider.bounds;

        // Origem: um ponto logo abaixo do "pé" do Player
        Vector2 origin = new Vector2(b.center.x, b.min.y - 0.02f);

        // Tamanho: caixa bem baixa, quase do tamanho da largura do Player
        Vector2 size = new Vector2(b.size.x * 0.9f, 0.06f);

        // Physics2D.OverlapBox retorna o collider da plataforma se houver alguma
        // - position: onde procurar (origin)
        // - size: tamanho da caixa
        // - angle: 0 (sem rotação)
        // - layerMask: só procura em objetos da Layer Platform
        return Physics2D.OverlapBox(origin, size, 0f, platformLayer);
    }

    // Corrotina que permite atravessar a plataforma encontrada
    IEnumerator DropThrough(Collider2D platform)
    {
        // Garante que não estamos tentando escalar ao mesmo tempo
        isClimbing = false;

        // Ativa a gravidade normal (caso estivesse desativada por climb)
        rig.gravityScale = 3.5f;

        // Ignora a colisão entre o collider do Player e o collider da plataforma
        Physics2D.IgnoreCollision(playerCollider, platform, true);

        // Dá um pequeno empurrão para baixo, garantindo que entre no Effector
        rig.linearVelocity = new Vector2(rig.linearVelocity.x, -5f);

        // Espera alguns "steps" de física (FixedUpdate) para o Player atravessar
        for (int i = 0; i < 16; i++)
            yield return new WaitForFixedUpdate();

        // Reativa a colisão normalmente
        Physics2D.IgnoreCollision(playerCollider, platform, false);
    }


    void OnAttack()
    {
        if(Input.GetMouseButtonDown(0) && canAttack)
        {
            Collider2D hit = Physics2D.OverlapCircle(pointAttack.position, radius, enemyLayer);

            if (hit != null)
            {
                //Inimigo sofre o ataque
            }

            isAttacking = true;

            canAttack = false;

            StartCoroutine(AttackRoutine());

        }
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(attackAnimDuration);

        isAttacking = false;

        float remaining = attackCooldown - attackAnimDuration;

        if (remaining > 0f)
        {
            yield return new WaitForSeconds(remaining);
        }

        canAttack = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(pointAttack.position, radius);

        if (playerCollider == null) return;
        var b = playerCollider.bounds;
        Vector2 origin = new Vector2(b.center.x, b.min.y - 0.02f);
        Vector2 size = new Vector2(b.size.x * 0.9f, 0.06f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(origin, size);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 8)
        {
            isJumping = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Climb"))
        {
            canClimb = true;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Climb"))
        {
            canClimb = false;
        }
    }

}

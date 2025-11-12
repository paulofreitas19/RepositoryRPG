using System.Collections;
using UnityEngine;

public class Dino : MonoBehaviour
{
    private Rigidbody2D rig;
    [SerializeField] private bool isFront;
    private Vector2 direction;
    private Animator anim;

    public float speed;
    public float maxVision;
    public Transform point;
    public Transform behind;
    public bool isRight;
    private bool isAttacking;
    public float attackDistance;
    public float health;

    private bool isInvulnerable;
    private bool isStunned;
    private SpriteRenderer spriteRenderer;
    private float blinkTimer;
    [SerializeField] private float blinkInterval = 0.2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (isRight)
        {
            transform.eulerAngles = new Vector2(0, 0);
            direction = Vector2.right;
        }

        else
        {
            transform.eulerAngles = new Vector2(0, 180);
            direction = Vector2.left;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        GetPlayer();
        OnMove();
    }

    private void OnMove()
    {
        // ✅ Se estiver atacando, não mexe em nada de movimento
        if (isAttacking)
        {
            rig.linearVelocity = Vector2.zero;
            return;
        }

        if (!isFront)
        {
            // Para movimento e animação de correr
            rig.linearVelocity = Vector2.zero;
            anim.SetInteger("transition", 0);
            return;
        }

        anim.SetInteger("transition", 1);

        if (isRight)
        {
            transform.eulerAngles = new Vector2(0, 0);
            direction = Vector2.right;
            rig.linearVelocity = new Vector2(speed, rig.linearVelocity.y);
        }

        else
        {
            transform.eulerAngles = new Vector2(0, 180);
            direction = Vector2.left;
            rig.linearVelocity = new Vector2(-speed, rig.linearVelocity.y);
        }

    }

    void GetPlayer()
    {
        int mask = LayerMask.GetMask("Player");

        RaycastHit2D hit = Physics2D.Raycast(point.position, direction, maxVision, mask);

        if(hit.collider == null)
        {
            isFront = false;
            rig.linearVelocity = Vector2.zero;
        }

        else
        {
            if (hit.transform.CompareTag("Player"))
            {
                isFront = true;

                float distance = Vector2.Distance(transform.position, hit.transform.position);

                if (distance <= attackDistance)
                {
                    isFront = false;
                    isAttacking = true;
                    rig.linearVelocity = Vector2.zero;

                    anim.SetInteger("transition", 2);

                    hit.transform.GetComponent<Player>().OnHit(0.2f);

                    return;
                }
            }
        }

        RaycastHit2D behindHit = Physics2D.Raycast(behind.position, -direction, maxVision, mask);

        if (behindHit.collider != null)
        {
            if (behindHit.transform.CompareTag("Player"))
            {
                //player est� nas costas do inimigo
                isRight = !isRight;
                isFront = true;
            }
        }

    }

    public void OnHit(float damage)
    {
        anim.SetTrigger("isHit");

        health -= damage;

        if (health <= 0.01f)
        {
            speed = 0;
            anim.SetTrigger("isDeath");
            Destroy(gameObject, 0.4f);
        }

        StartCoroutine(StunTime());

    }

    private IEnumerator StunTime()
    {
        isInvulnerable = true;
        isStunned = true;
        yield return new WaitForSeconds(1f);
        isStunned = false;
    }

    void HandleInvulnerabilityBlink()
    {
        if (isInvulnerable)
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

    public void EndAttack()
    {
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(point.position, direction * maxVision);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(behind.position, -direction * maxVision);
    }
}
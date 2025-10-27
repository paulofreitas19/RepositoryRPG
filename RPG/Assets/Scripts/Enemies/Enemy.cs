using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rig;
    private Animator anim;
    public Transform point;
    public float radius;
    private SpriteRenderer spriteRenderer;
    private float blinkTimer;
    [SerializeField] private float blinkInterval = 0.2f;
    public LayerMask layer;
    [SerializeField] private float health;
    [SerializeField] private float maxHealth = 1f;
    [SerializeField] private float speed;
    private bool isInvulnerable;
    private bool isStunned;

    public float Health
    {
        get { return health; }
        set { health = value; }
    }

    public float MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (isStunned)
        {
            rig.linearVelocity = Vector2.zero;
            return;
        }

        rig.linearVelocity = new Vector2(speed, rig.linearVelocity.y);
        OnCollision();
    }

    void OnCollision()
    {

        Collider2D hit = Physics2D.OverlapCircle(point.position, radius, layer);

        if (hit != null)
        {
            speed = -speed;

            if (transform.eulerAngles.y == 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }

            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(point.position, radius);
    }

}
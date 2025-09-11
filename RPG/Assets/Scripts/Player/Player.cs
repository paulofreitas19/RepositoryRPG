using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rig;

    [Header("Stats")]
    [SerializeField] private int health;
    [SerializeField] private float speed;
    private float initialSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float radius;
    [SerializeField] private float attackAnimDuration;
    [SerializeField] private float attackCooldown;

    [SerializeField] Transform pointAttack;
    [SerializeField] LayerMask enemyLayer;

    private bool isMoving;
    private bool isJumping;
    private bool doubleJumping;
    private bool isRunning;
    private bool isAttacking;
    private bool canAttack;

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
    }

    void FixedUpdate()
    {
        OnMove();
    }

    void OnMove()
    {
        if(isAttacking) return;

        float movement = Input.GetAxis("Horizontal");
        rig.linearVelocity = new Vector2(movement * speed, rig.linearVelocity.y);

        if (movement == 0 && !isJumping)
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
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isJumping)
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            isJumping = false;
        }
    }

}

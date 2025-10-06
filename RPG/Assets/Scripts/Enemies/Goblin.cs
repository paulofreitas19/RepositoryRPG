using UnityEngine;

public class Goblin : MonoBehaviour
{
    private Rigidbody2D rig;
    private bool isFront;
    private Vector2 direction;
    private Animator anim;

    public float speed;
    public float maxVision;
    public Transform point;
    public Transform behind;
    public bool isRight;
    public float attackDistance;
    public float hp;

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
        if (isFront)
        {
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

    }

    void GetPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(point.position, direction, maxVision);

        if (hit.collider != null)
        {
            if (hit.transform.CompareTag("Player"))
            {
                isFront = true;

                float distance = Vector2.Distance(transform.position, hit.transform.position);

                if (distance <= attackDistance)
                {
                    isFront = false;
                    rig.linearVelocity = Vector2.zero;

                    anim.SetInteger("transition", 2);

                    hit.transform.GetComponent<Player>().OnHit(0.2f);

                }
            }

        }

        RaycastHit2D behindHit = Physics2D.Raycast(behind.position, -direction, maxVision);

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

    public void OnHit()
    {
        anim.SetTrigger("hit");
        hp--;

        if (hp <= 0)
        {
            speed = 0;
            anim.SetTrigger("death");
            Destroy(gameObject, 1f);
        }

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
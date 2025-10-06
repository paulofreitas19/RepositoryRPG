using UnityEngine;

public class Spider : MonoBehaviour
{
    private Rigidbody2D rig;

    private Animator anim;

    public int hp;

    public float speed;

    public Transform point;

    public float radius;

    public LayerMask layer;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
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


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(point.position, radius);
    }

}
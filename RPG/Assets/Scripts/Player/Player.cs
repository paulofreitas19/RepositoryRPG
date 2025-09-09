using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rig;

    [Header("Stats")]
    [SerializeField] private int health;
    [SerializeField] private float speed;
    [SerializeField] private float initialSpeed;
    [SerializeField] private float speedRun;
    [SerializeField] private float jumpForce;
    private bool isMovement;
    private bool isJumping;

    public bool IsMovement
    {
        get { return isMovement; }
        set { isMovement = value; }
    }

    public bool IsJumping
    {
        get { return isJumping; }
        set { isJumping = value; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        initialSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        OnJump();
    }

    void FixedUpdate()
    {
        OnMove();
    }

    void OnMove()
    {
        float movement = Input.GetAxis("Horizontal");
        rig.linearVelocity = new Vector2(movement * speed, rig.linearVelocity.y);

        if (movement == 0)
        {
            isMovement = false;
        }

        if (movement < 0)
        {
            isMovement = true;
            transform.eulerAngles = new Vector2(0, 180);
        }

        if(movement > 0)
        {
            isMovement = true;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            isJumping = false;
        }
    }

}

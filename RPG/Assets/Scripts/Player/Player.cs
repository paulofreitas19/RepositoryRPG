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
    private bool isMoving;
    private bool isJumping;
    private bool isRunning;

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
        OnRun();
    }

    void FixedUpdate()
    {
        OnMove();
    }

    void OnMove()
    {
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
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = runSpeed;
            isRunning = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = initialSpeed;
            isRunning = true;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            isJumping = false;
        }
    }

}

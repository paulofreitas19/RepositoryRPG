using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rig;

    [Header("Stats")]
    [SerializeField]private int health;
    [SerializeField]private float speed;
    [SerializeField]private float initialSpeed;
    [SerializeField]private float speedRun;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

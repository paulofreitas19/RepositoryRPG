using UnityEngine;

public class FloatingMovingPlatform : MonoBehaviour
{
    [Header("Points")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("Move Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private bool loop = true;

    [Header("Float Settings")]
    [SerializeField] private float floatAmplitude = 0.2f;
    [SerializeField] private float floatSpeed = 2f;

    [Header("Tilt Settings")]
    [SerializeField] private float maxTilt = 5f;   // ângulo máximo (em graus)
    [SerializeField] private float tiltSmooth = 2f; // suavidade da transição

    [Header("Player Settings")]
    [SerializeField] private bool playerStick = true;

    private Transform targetPoint;
    private Vector3 basePosition;
    private float floatTimer;

    void Start()
    {
        targetPoint = pointB;
        basePosition = transform.position;
    }

    void Update()
    {
        MoveBetweenPoints();
        ApplyFloatEffect();
        ApplyTiltEffect();
    }

    void MoveBetweenPoints()
    {
        basePosition = Vector3.MoveTowards(
            basePosition,
            targetPoint.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(basePosition, targetPoint.position) < 0.05f)
        {
            targetPoint = (targetPoint == pointB) ? pointA : pointB;

            if (!loop && targetPoint == pointA)
                enabled = false;
        }
    }

    void ApplyFloatEffect()
    {
        floatTimer += Time.deltaTime * floatSpeed;
        float offsetY = Mathf.Sin(floatTimer) * floatAmplitude;

        transform.position = new Vector3(
            basePosition.x,
            basePosition.y + offsetY,
            basePosition.z
        );
    }

    void ApplyTiltEffect()
    {
        // direção horizontal → -1 (esquerda), 0 (parado), +1 (direita)
        float direction = Mathf.Sign(targetPoint.position.x - transform.position.x);

        // alvo de inclinação baseado na direção
        float targetTilt = direction * maxTilt;

        // interpolação suave para evitar mudanças bruscas
        float tilt = Mathf.LerpAngle(
            transform.eulerAngles.z,
            targetTilt,
            Time.deltaTime * tiltSmooth
        );

        transform.rotation = Quaternion.Euler(0, 0, tilt);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerStick && collision.gameObject.CompareTag("Player"))
            collision.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (playerStick && collision.gameObject.CompareTag("Player"))
            collision.transform.SetParent(null);
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class Potion : MonoBehaviour
{
    private Player player;

    [SerializeField] private AudioClip potionSound;

    [Header("Floating Settings")]
    [SerializeField] private float floatAmplitude = 0.2f; // altura da oscilação
    [SerializeField] private float floatSpeed = 2f;       // velocidade do flutuar

    [Header("Tilt Settings (balanço leve em vez de rotação completa)")]
    [SerializeField] private float tiltAmount = 5f;       // ângulo máximo
    [SerializeField] private float tiltSpeed = 2f;        // velocidade do balanço

    [Header("Glow Settings")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color glowColor = Color.yellow;
    [SerializeField] private float glowSpeed = 2f;
    [SerializeField] private float glowIntensity = 0.4f;

    private Vector3 startPos;
    private Color baseColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindObjectOfType<Player>();
        startPos = transform.position;
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        baseColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        FloatMotion();
        TiltMotion();
        GlowPulse();
    }

    void FloatMotion()
    {
        float y = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPos.x, startPos.y + y, startPos.z);
    }

    void TiltMotion()
    {
        float tilt = Mathf.Sin(Time.time * tiltSpeed) * tiltAmount;
        transform.rotation = Quaternion.Euler(0, 0, tilt);
    }

    void GlowPulse()
    {
        float t = (Mathf.Sin(Time.time * glowSpeed) + 1f) / 2f;
        spriteRenderer.color = Color.Lerp(baseColor, glowColor, t * glowIntensity);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            player.Health = player.MaxHealth;
            AudioController.instance.PlayAndDestroy(potionSound, transform.position, null);
            Destroy(gameObject);
        }
    }
}

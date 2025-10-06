using UnityEngine;

public class PlayerPos : MonoBehaviour
{
    private Transform player;
    public static PlayerPos instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject); // Corrigido: destruir o duplicado, não o instance
    }

    private void Start()
    {
        // Define o Player apenas uma vez
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void CheckPoint()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        Vector3 playerPos = transform.position;
        playerPos.z = 0;

        player.position = playerPos;
    }
}

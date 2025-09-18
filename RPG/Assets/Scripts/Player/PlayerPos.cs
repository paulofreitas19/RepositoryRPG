using UnityEngine;

public class PlayerPos : MonoBehaviour
{
    private Transform player;

    public static PlayerPos instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(instance);
        }

        if(player != null)
        {
            CheckPoint();
        }
    }

    public void CheckPoint()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        Vector3 playerPos = transform.position;
        playerPos.z = 0;

        player.position = playerPos;
    }
}

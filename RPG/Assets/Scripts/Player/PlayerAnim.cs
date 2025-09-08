using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Player player;
    private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<Player>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        OnMove();
    }

    void OnMove()
    {
        if (player.IsMovement)
        {
            anim.SetInteger("transition", 1);
        }

        if(!player.IsMovement)
        {
            anim.SetInteger("transition", 0);
        }
        
    }
}

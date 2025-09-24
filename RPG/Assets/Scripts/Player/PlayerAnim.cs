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
        if (player.IsDeath)
        {
            anim.SetTrigger("isDeath");
        }

        else if (player.IsHit)
        {
            anim.SetTrigger("isHit");
            //player.IsHit = false;
        }

        else if (player.IsAttacking)
        {
            anim.SetInteger("transition", 2);
        }

        else if (player.IsClimbing)
        {
            anim.SetInteger("transition", 3);
        }

        else if (player.IsJumping)
        {
            anim.SetInteger("transition", 4);
        }

        else if (player.IsRunning)
        {
            anim.SetInteger("transition", 1);
        }

        else
        {
            anim.SetInteger("transition", 0);
        } 
    }
}

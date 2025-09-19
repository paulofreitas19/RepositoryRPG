using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private AudioClip coinSound;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            coll.GetComponent<Player>().CurrentGold++;

            // chama o controlador para tocar som e destruir a moeda
            AudioController.instance.PlayAndDestroy(coinSound, transform.position, gameObject);
        }
    }
}

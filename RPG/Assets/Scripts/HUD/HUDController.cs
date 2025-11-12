using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class HUDController : MonoBehaviour
{
    private Player player;

    [SerializeField] private Image healthBar;
    [SerializeField] private Text currentGoldText;

    private Spider spider;

    [SerializeField] private Image enemyBar;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindObjectOfType<Player>();
        healthBar.fillAmount = player.MaxHealth;
        spider = FindFirstObjectByType<Spider>();
        enemyBar.fillAmount = spider.MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = player.Health;
        enemyBar.fillAmount = spider.Health;
        currentGoldText.text = "x " + player.CurrentGold.ToString();
    }
}

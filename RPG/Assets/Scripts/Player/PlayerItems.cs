using UnityEngine;

public class PlayerItems : MonoBehaviour
{
    [SerializeField] private int currentGold = 0;
    private int totalGold = 100;

    public int CurrentGold
    {
        get { return currentGold; }
        set { currentGold = value; }
    }

    public int TotalGold
    {
        get { return totalGold; }
        set { totalGold = value; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

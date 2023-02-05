using TMPro;
using UnityEngine;

public class PaymentManager : MonoBehaviour
{
    public static PaymentManager instance;
    public int money, total;
    [SerializeField] TextMeshProUGUI totalText;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] LifeCost[] lifeCosts;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        total = money;
        lifeCosts = FindObjectsOfType<LifeCost>();
    }
    public void UpdateMoney() {
        moneyText.text = "Momney: " + money;
    }

    public void UpdateTotal(LifeCost lifeCost)
    {
        if (lifeCost.isPaid) total -= lifeCost.cost;
        else total += lifeCost.cost;
        totalText.text = "$" + total;
    }

    public void Pay()
    {
        if (total < 0) return;
        foreach  (LifeCost item in lifeCosts)
        {
            if (!item.isPaid) item.LoseChance();             
        }
        money = total;
        NextDay();
    }
    void NextDay()
    {
        GameManager.instance.startGame();
    }
}

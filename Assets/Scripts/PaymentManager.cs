using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PaymentManager : MonoBehaviour
{
    public static PaymentManager instance;
    public int moneyPerMission, money, total;
    [SerializeField] TextMeshProUGUI totalText;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] LifeCost[] lifeCosts;
    [SerializeField] AudioClip audioClipWin, audioClipLose;
    [SerializeField] AudioSource source;
    bool lose= false;

    

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        moneyPerMission = 30;

        //lifeCosts = FindObjectsOfType<LifeCost>();
        MailMission.instance.onMissionCompleteAction += AddMoney;

    }
    public void AddMoney()
    {
        money += moneyPerMission;
        UpdateMoney();
        UpdateTotal();
    }
    public void UpdateMoney() 
    {
        moneyText.text = "Dinero: " + money;
    }

    
    public void UpdateTotal()
    {
        total = money;
        totalText.text = "$" + total;
    }
    public void UpdateTotal(LifeCost lifeCost)
    {
        if (lifeCost.isPaid) total -= lifeCost.cost;
        else total += lifeCost.cost;
        totalText.text = "$" + total;
    }

    public void Pay()
    {
        if (total < 0)
        {
            source.PlayOneShot(audioClipLose);
            return;
        }
        foreach  (LifeCost item in lifeCosts)
        {
            if (!item.isPaid) item.LoseChance();
            if (item.chances <= 0) lose = true;
        }
        money = total;
        if(!lose)
        NextDay();
    }
    void NextDay()
    {
        source.PlayOneShot(audioClipWin);
        GameManager.instance.startGame();
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PaymentManager : MonoBehaviour
{
    public static PaymentManager instance;
    public int moneyPerMission, money, total;
    [SerializeField] TextMeshProUGUI totalText;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] LifeCost[] lifeCosts;
    [SerializeField] Toggle[] toggles;
    [SerializeField] AudioClip audioClipWin, audioClipLose;
    [SerializeField] AudioSource source;
    [SerializeField] GameObject debt;
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
        total = (!GameManager.instance.playTutorial) ? money : money-100;
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
        foreach (var item in toggles)
        {
            item.isOn = false;
        }
        
        if(!lose)
        NextDay();
    }
    void NextDay()
    {
        debt.SetActive(false);
        source.PlayOneShot(audioClipWin);
        GameManager.instance.startGame();
    }
}

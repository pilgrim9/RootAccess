using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeCost : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI costText;
    public bool isPaid;
    public int chances, cost;
    public GameObject[] chancesVisual;
    public string loseMessage;

    private void Start()
    {
        UpdateCost();
    }
    void Lose()
    {
        LoseManager.Instance.Lose(loseMessage);
    } 
    public void LoseChance()
    {
        chances--;
        chancesVisual[chances].SetActive(false);
        if (chances <= 0) Lose();
    }
    public void togglePaid(bool value)
    {
        isPaid = value;
        PaymentManager.instance.UpdateTotal(this);
    }
    public void UpdateCost()
    {
        costText.text = "$" + cost;
    }
}

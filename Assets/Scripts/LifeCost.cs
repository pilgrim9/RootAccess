using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeCost : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI costText;
    public bool isPaid;
    public int chances, cost;

    private void Start()
    {
        UpdateCost();
    }
    void Lose()
    {
        SceneManager.LoadScene(0);
    } 
    public void LoseChance()
    {
        chances--;
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

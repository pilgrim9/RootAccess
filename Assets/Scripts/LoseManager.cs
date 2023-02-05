using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LoseManager : MonoBehaviour
{
    public static LoseManager Instance;
    public UnityEvent OnLose;
    public TextMeshProUGUI loseText;
    private void Awake()
    {
        Instance = this;
    }
    private void PlayAnim()
    {
        GetComponent<Animation>().Play();
    }
    public void Lose(string message)
    {
        loseText.text = message;
        PlayAnim();
    }
    public void AnimFinish()
    {
        OnLose.Invoke();
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LoseManager : MonoBehaviour
{
    public static LoseManager Instance;
    public UnityEvent OnLose;
    public TextMeshProUGUI loseText;
    public AudioSource ambient;
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
        ambient.mute= true;
    }
    public void AnimFinish()
    {
        OnLose.Invoke();
    }
}

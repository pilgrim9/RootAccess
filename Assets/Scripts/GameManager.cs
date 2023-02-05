using UnityEngine;
using UnityEngine.Events;
using TMPro;
public class GameManager : MonoBehaviour
{
    public int minutesPerDay = 3;
    public float elapsedTime = 0f;
    public bool gameStarted = false;

    public TMP_Text timerText;
    public UnityEvent onGameStart;
    public UnityEvent onGameEnd;

    private void Start()
    {
        startGame();
    }
    public void startGame()
    {
        gameStarted = true;
        onGameStart.Invoke();
        MailMission.instance.CreateMission();
    }
    private void Update()
    {
        if (gameStarted)
        {
            timerText.text = getTime();
            elapsedTime += Time.deltaTime;
            if (elapsedTime > minutesPerDay * 60)
            {
                endGame();
            }
        }
    }

    public void endGame()
    {
        gameStarted = false;
        onGameEnd.Invoke();
    }

    public string getTime()
    {
        float percentageElapsed = elapsedTime / (minutesPerDay * 60f);
        float ElapsedMinutes = 8 * 60 * percentageElapsed;
        float ElapsedHours = Mathf.Floor(ElapsedMinutes / 60f);
        float extraMinutes = Mathf.Floor(ElapsedMinutes - ElapsedHours*60) ;
        return ((ElapsedHours + 9) == 9 ? "09" : ElapsedHours + 9) + ":" + (extraMinutes < 10 ? "0" + extraMinutes : extraMinutes);
    }
}
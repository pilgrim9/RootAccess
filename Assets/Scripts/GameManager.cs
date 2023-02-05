using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public int minutesPerDay = 3;
    public float secondsRemaining;
    public bool gameStarted = false;
    
    public UnityEvent onGameStart;
    public UnityEvent onGameEnd;

    private void Start()
    {
        startGame();
    }
    public void startGame()
    {
        gameStarted = true;
        secondsRemaining = minutesPerDay * 60;
        onGameStart.Invoke();
        MailMission.instance.CreateMission();
    }
    private void Update()
    {
        if (gameStarted)
        {
            secondsRemaining -= Time.deltaTime;
            if (secondsRemaining < 0)
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
        float timeElapsed = minutesPerDay * 60 - secondsRemaining;
        float simulationSecondsElapsed = 8 * 60 * 60 * timeElapsed / (minutesPerDay * 60);
        int hours = Mathf.FloorToInt(simulationSecondsElapsed / 60 / 60) + 4;
        int minutes = Mathf.FloorToInt(simulationSecondsElapsed / 60) - hours * 60;
        return hours + ":" + minutes;
    }
}

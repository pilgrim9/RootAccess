using UnityEngine;
using UnityEngine.Events;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float minutesPerDay = 0.1f;
    public float elapsedTime = 0f;
    public bool gameStarted = false;

    public TMP_Text timerText;
    public UnityEvent onDayStart;
    public UnityEvent onDayEnd;
    public MissionSO FirstMission;
    public bool playTutorial = true;

    public AudioSource audioSource;
    public AudioClip lowTime;

    public Animation fade;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        startTutorial();
    }

    private void startTutorial()
    {
        if (playTutorial)
        {
            MailMission.instance.StartFirstMission();
            onDayStart.Invoke();
        }
        else
        {
            startGame();
        }
    }
    public void startGame()
    {
        playTutorial = false;
        elapsedTime = 0f;
        gameStarted = true;
        onDayStart.Invoke();
        MailMission.instance.CreateMission();
    }
    private void Update()
    {
        if (!gameStarted) return;
        if (playTutorial) return;
        
        timerText.text = getTime();
        elapsedTime += Time.deltaTime;
        if (elapsedTime > minutesPerDay * 60)
        {
            endGame();
        }
        float percentageElapsed = elapsedTime / (minutesPerDay * 60f);
        if (percentageElapsed > 0.8f)
        {
            if (audioSource.isPlaying || !gameStarted) return;
            audioSource.clip = lowTime;
            audioSource.Play();
        }
    }

    public void endGame()
    {
        audioSource.Stop();
        gameStarted = false;
        onDayEnd.Invoke();
        fade.Play();
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
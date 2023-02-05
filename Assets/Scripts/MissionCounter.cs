using System;
using TMPro;
using UnityEngine;

public class MissionCounter : MonoBehaviour
{
    public int missionsCompleated;

    public TextMeshProUGUI countText;
    public Animation anim;

    private void Start()
    {
        MailMission.instance.onMissionCompleteAction += OnMissionCompleated;
    }
    public void OnMissionCompleated() {
        missionsCompleated++;
        UpdateMissionCount();
        PlayAnim();
    }
    public void ResetMissionCount()
    {
        missionsCompleated = 0;
        UpdateMissionCount();
    }
    public void PlayAnim()
    {
        anim.Play();
    }
    private void UpdateMissionCount()
    {
        countText.text = missionsCompleated.ToString();
    }
}

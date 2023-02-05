using System.Collections;
using UnityEngine;

public class EventSound : MonoBehaviour
{
    [SerializeField] AudioSource source,sourceMission;
    [SerializeField] AudioClip[] clips;
    [SerializeField] AudioClip missionSound;
    [SerializeField] float minTime, maxTime;

    void Start()
    {
        StartSound();
        clips = Resources.LoadAll<AudioClip>("Sounds/Effects");
    }

    IEnumerator RandomSound(float time)
    {
        int randomClip = Random.Range(0, clips.Length);
        yield return new WaitForSeconds(time);
        source.clip = clips[randomClip];
        source.Play();
        StartSound();
    }

    void StartSound()
    {
        float randomTime = Random.Range(minTime, maxTime);
        StartCoroutine(RandomSound(randomTime));
    }

    void MissionCompleteSound() 
    {
        sourceMission.PlayOneShot(missionSound);
    }

}

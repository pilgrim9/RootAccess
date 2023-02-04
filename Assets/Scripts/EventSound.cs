using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSound : MonoBehaviour
{
    [SerializeField] AudioSource source;
    AudioClip[] clips;
    [SerializeField] float minTine, maxTime;

    void Start()
    {
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
        float randomTime = Random.Range(minTine, maxTime);
        StartCoroutine(RandomSound(randomTime));
    }
}

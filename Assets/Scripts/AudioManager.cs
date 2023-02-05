using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    public AudioClip day;
    public AudioClip recibe;

    public AudioSource audioSource;

    public UnityEvent OnAnimFinish;

    public void PlayDay()
    {
        audioSource.clip = day;
        audioSource.Play();
    }
    public void PlayRecibe()
    {
        audioSource.clip = recibe;
        audioSource.Play();
    }
    public void AnimFinish()
    {
        OnAnimFinish.Invoke();

    }

}

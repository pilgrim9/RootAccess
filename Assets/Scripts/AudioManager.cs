using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip day;
    public AudioClip recibe;

    public AudioSource audioSource;

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

}

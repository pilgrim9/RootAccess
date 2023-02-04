using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    void ChangeVolume() => audioSource.volume = PlayerPrefs.GetFloat("Volume");
}

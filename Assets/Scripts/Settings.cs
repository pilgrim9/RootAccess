using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    public void ChangeVolume() => audioSource.volume = PlayerPrefs.GetFloat("Volume");
}

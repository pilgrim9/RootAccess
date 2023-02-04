using UnityEngine;

public class Settings : MonoBehaviour
{ 
    [SerializeField] AudioSource[] audioSources;

    private void Start()
    {
        audioSources = FindObjectsOfType<AudioSource>();
    }

    public void ChangeVolume() 
    {
        foreach (AudioSource item in audioSources)
        {
            item.volume = PlayerPrefs.GetFloat("Volume");
        }
        
    }
}

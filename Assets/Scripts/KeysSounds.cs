using UnityEngine;

public class KeysSounds : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip enter;
    public AudioClip[] keyBoard, mouse;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            audioSource.PlayOneShot(enter);
        } 
        else if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            int randomMouse = Random.Range(0, mouse.Length);
            audioSource.PlayOneShot(keyBoard[randomMouse]);
        }
        else if (Input.anyKeyDown)
        {
            int randomKey = Random.Range(0, keyBoard.Length);
            audioSource.PlayOneShot(keyBoard[randomKey]);
        }
    }
}
using UnityEngine;

public class KeysSounds : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip enter, mouse;
    public AudioClip[] keyBoard;
    
    private void Update()
    {
        if(Input.anyKeyDown && !Input.GetKeyDown(KeyCode.KeypadEnter) && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
        {
            int randomKey = Random.Range(0, keyBoard.Length);
            audioSource.PlayOneShot(keyBoard[randomKey]);
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            audioSource.PlayOneShot(enter);
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            audioSource.PlayOneShot(mouse);
        }
    }
}
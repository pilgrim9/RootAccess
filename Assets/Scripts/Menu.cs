using UnityEngine.SceneManagement;  
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] Slider volume;

    void StartGame() => SceneManager.LoadScene("Game");

    void ExitGame() => Application.Quit();

    void SaveVolume() => PlayerPrefs.SetFloat("Volume", volume.value);

}

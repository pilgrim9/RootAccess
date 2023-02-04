using UnityEngine.SceneManagement;  
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] Slider volume;

    public void StartGame() => SceneManager.LoadScene("Game");

    public void ExitGame() => Application.Quit();

    public void SaveVolume() => PlayerPrefs.SetFloat("Volume", volume.value);

}

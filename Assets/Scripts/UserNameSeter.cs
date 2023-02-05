using TMPro;
using UnityEngine;

public class UserNameSeter : MonoBehaviour
{
    private void Start()
    {
        GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("Name");
    }
}

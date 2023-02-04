using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Username : MonoBehaviour
{
    [SerializeField] TextMeshPro nameText, idText;
    [SerializeField] GameObject namePanel, menuPanel;
    void SaveName() 
    { 
        PlayerPrefs.SetString("Name", nameText.text);
        idText.text = PlayerPrefs.GetString("Name");
        namePanel.SetActive(false);
        menuPanel.SetActive(true);
    }
}

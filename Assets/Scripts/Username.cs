using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Username : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText, idText,emailText;
    [SerializeField] GameObject namePanel, menuPanel;
    public void SaveName()
    {
        PlayerPrefs.SetString("Name", nameText.text.ToUpper());
        idText.text = PlayerPrefs.GetString("Name");
        emailText.text = PlayerPrefs.GetString("Name").ToLower()+ "@gmail.com";
        namePanel.SetActive(false);
        menuPanel.SetActive(true);
    }
}

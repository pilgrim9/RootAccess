using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Username : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI  idText,emailText;
    [SerializeField] TMP_InputField nameText;
    private void Start()
    {
        LoadName();
    }
    public void SaveName()
    {
        PlayerPrefs.SetString("Name", nameText.text.ToUpper());
        idText.text = PlayerPrefs.GetString("Name");
        emailText.text = PlayerPrefs.GetString("Name").ToLower()+ "@gmail.com";
    }
    public void LoadName()
    {
        idText.text = PlayerPrefs.GetString("Name");
        emailText.text = PlayerPrefs.GetString("Name").ToLower() + "@gmail.com";
        nameText.text = PlayerPrefs.GetString("Name");
    }
}

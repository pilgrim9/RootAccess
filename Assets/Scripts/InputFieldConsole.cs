using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputFieldConsole : MonoBehaviour
{
    TMP_InputField inputField;
    private void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onSubmit.AddListener(SubmitInput);
    }

    public void SubmitInput(string arg0)
    {
        inputField.ActivateInputField();
    }
}

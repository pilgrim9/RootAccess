using System;
using TMPro;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public TMP_Text text;
    public TMP_InputField InputField;

    public string placeholder = "Input a command...";
    public string consolePlaceholder = "Bienvenido!";
    private void Start()
    {
        text.text = consolePlaceholder;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("enter"))
        {
            string input = InputField.text;
            text.text += CommandLine.instance.InputCommand(input);
            InputField.text = placeholder;
        }
    }
}

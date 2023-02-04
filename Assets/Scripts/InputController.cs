using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    public TMP_Text text;
    public TMP_InputField InputField;

    public string consolePlaceholder = "Bienvenido!";
    private void Start()
    {
        text.text = consolePlaceholder;
        text.text += CommandLine.instance.InputCommand("List");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("enter");
            HandleInput();
        }
    }

    public void HandleInput()
    {
        string input = InputField.text;
        text.text += CommandLine.instance.InputCommand(input);
        InputField.text = "";
    }
}

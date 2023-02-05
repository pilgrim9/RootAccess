using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    public TMP_Text text;
    public TMP_InputField InputField;

    public string consolePlaceholder = "Buen dia!";

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        text.text = consolePlaceholder;
        if (!GameManager.instance.playTutorial)
        {
            FileSystem.instance.currentFolder = FileSystem.instance.root;
            text.text += CommandLine.instance.InputCommand("List");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
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

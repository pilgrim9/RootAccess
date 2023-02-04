using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.PackageManager;
using UnityEngine;

public class CommandLine : MonoBehaviour
{
    public static CommandLine instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
    }

    private Dictionary<string, string> commands = new Dictionary<string, string>()
    {
        { "open", nameof(Open) },
        { "back", nameof(Back) },
        { "install", nameof(Install) },
        { "cut", nameof(Cut)},
        { "paste", nameof(Paste)},
        // { "download", nameof(Download)},
    };

    private string output = "";
    
    public string InputCommand(string input)
    {
        clearOutput();
        string[] parameters = input.Split(" ");
        if (parameters.Length > 2)
        {
            output = "Too many arguments";
            return output;
        }
        
        string command = parameters[0].ToLower();
        string parameter = parameters.Length == 1? "" : parameters[1].ToLower();

        if (!commands.Keys.Contains(command))
        {
            output = "Command does not exist";
            return output;
        }
        Type thisType = this.GetType();
        MethodInfo theMethod = thisType.GetMethod(commands[command]);
        theMethod.Invoke(this, new[] { parameter });
        return output;
    }

    private void clearOutput()
    {
        output = "";
    }

    private void Open(string parameter)
    {
        if (ParameterVoid(parameter))
        {
            output = "Please input a folder name";
            return;
        }

        if (!FileSystem.instance.currentFolder.Contains(parameter))
        {
            output = "Folder doesn't exist in current folder";
        }

        OSFolder folder = (OSFolder)FileSystem.instance.currentFolder.Get(parameter);
        FileSystem.instance.currentFolder = folder;
        List();
    }
    
    private void Install(string[] parameters)
    {
        // not implemented
    }
    
    private void Cut(string parameter)
    {
        if (FileSystem.instance.currentFolder.Contains(parameter))
        {
            FileSystem.instance.clipboard = FileSystem.instance.currentFolder.Cut(parameter);
            output = "Cut file " + parameter;
            List();
        }
        else
        {
            output = "File does not exist in this folder";
        }
    }
    private void Paste(string parameters)
    {
        if (FileSystem.instance.clipboard != null)
        {
            FileSystem.instance.currentFolder.Add(FileSystem.instance.clipboard);
            FileSystem.instance.clipboard = null;
            output = "File Pasted";
            List();
        }
        else
        {
            output = "File does not exist in this folder";
        }
    }
    private void Back(string parameter)
    {
        if (!ParameterVoid(parameter))
        {
            output = "Back doesn't require parameters!";
            return;
        }
        FileSystem.instance.currentFolder = FileSystem.instance.currentFolder.ParentFolder;
        List();
    }

    private void List()
    {
        List("");
    }
    private void List(string parameter)
    {
        if (!ParameterVoid(parameter))
        {
            output = "List doesn't require parameters!";
            return;
        }

        output += "You are in " + FileSystem.instance.currentFolder.name;
        
        output += "\n This folder contains "+FileSystem.instance.currentFolder.subfolders.Count+" folders:";
        foreach (var folder in FileSystem.instance.currentFolder.subfolders)
        {
            output += "\n" + folder.name;
        }

        output += "\n This folder contains "+FileSystem.instance.currentFolder.files.Count+" files:";
        foreach (var file in FileSystem.instance.currentFolder.files)
        {
            output += "\n" + file.name;
        }
    }

    private bool ParameterVoid(string parameter)
    {
        return (parameter == "");
    }
}

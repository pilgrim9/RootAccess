using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class CommandLine : MonoBehaviour
{
    public static CommandLine instance;

    public string[] installers =
    {
        "internet",
        "quill",
        "studio",
        "workbench",
        "",
        "accounting"
    };

    private void Awake()
    {
        instance = this;
    }

    private Dictionary<string, string> commands = new Dictionary<string, string>()
    {
        { "open", nameof(Open) },
        { "back", nameof(Back) },
        { "install", nameof(Install) },
        { "cut", nameof(Cut)},
        { "paste", nameof(Paste)},
        { "list", nameof(List)},
        { "download", nameof(Download)},
    };

    private string output = "";
    
    public string InputCommand(string input)
    {
        clearOutput();
        string[] parameters = input.Split(" ");
        if (parameters.Length > 2)
        {
            output = "Too many arguments";
            return getOutput(input);
        }
        
        string command = parameters[0].ToLower();
        string parameter = parameters.Length == 1? "" : parameters[1].ToLower();

        if (!commands.Keys.Contains(command))
        {
            output = "Command "+command+" does not exist";
            return getOutput( input);
        }
        Type thisType = this.GetType();
        MethodInfo theMethod = thisType.GetMethod(commands[command]);
        Debug.Log(command + " command function"  + commands[command]);
        Debug.Log("method name" + theMethod);
        theMethod.Invoke(this, new[] { parameter });
        return getOutput( input);
    }

    private string getOutput(string input)
    {
        return "\n"+"Current folder:" + FileSystem.instance.currentFolder.FolderPath+"> "+input+
               "\n"+output;
    }
    private void clearOutput()
    {
        output = "";
    }

    public void Open(string parameter)
    {
        if (ParameterVoid(parameter))
        {
            output = "Please input a folder name";
            return;
        }

        Debug.Log(parameter);
        if (!FileSystem.instance.currentFolder.ContainsFolder(parameter))
        {
            output = "Folder \"" + parameter + "\" doesn't exist in current folder";
            return;
        }

        OSFolder folder = FileSystem.instance.currentFolder.GetFolder(parameter);
        Debug.Log(folder);
        FileSystem.instance.currentFolder = folder;
        List("");
    }

    public void Download(string parameter)
    {
        if (ParameterVoid(parameter))
        {
            output = "Please specify which installer to download \n" +
                     "Available downloads:\n";
            foreach (var name in installers)
            {
                output += name+"\n";
            }
        }
    }
    
    public void Install(string parameter)
    {
        if (!parameter.EndsWith(".exe"))
        {
            output = "You can only install files that end in .exe. ";
            return;
        }
        if (FileSystem.instance.currentFolder.ContainsFile(parameter))
        {
            output = "The file doesn't exist. You may need to download it first.";
            return;
        }

        output = parameter + " app installed!";
    }
    
    public void Cut(string parameter)
    {
        if (FileSystem.instance.currentFolder.ContainsFile(parameter))
        {
            FileSystem.instance.clipboard = FileSystem.instance.currentFolder.Cut(parameter);
            output = "Cut file " + parameter;
            List("");
        }
        else
        {
            output = "File does not exist in this folder";
        }
    }
    public void Paste(string parameters)
    {
        if (FileSystem.instance.clipboard != null)
        {
            FileSystem.instance.currentFolder.Add(FileSystem.instance.clipboard);
                FileSystem.instance.clipboard = null;
            output = "File Pasted";
            List("");
        }
        else
        {
            output = "File does not exist in this folder";
        }
    }
    public void Back(string parameter)
    {
        if (!ParameterVoid(parameter))
        {
            output = "Back doesn't require parameters!";
            return;
        }

        if (FileSystem.instance.currentFolder.ParentFolder == null)
        {
            output = "root folder doesn't have a parent folder";
        }
        FileSystem.instance.currentFolder = FileSystem.instance.currentFolder.ParentFolder;
        List("");
    }

   
    public void List(string parameter)
    {
        if (!ParameterVoid(parameter))
        {
            output = "The list command doesn't require parameters";
            return;
        }

        Debug.Log(FileSystem.instance.currentFolder.getName());
        output += "This folder contains "+FileSystem.instance.currentFolder.subfolders.Count+" folders:";
        foreach (var folder in FileSystem.instance.currentFolder.subfolders)
        {
            output += "\n -" + folder.getName();
        }

        output += "\nThis folder contains "+FileSystem.instance.currentFolder.files.Count+" files:";
        foreach (var file in FileSystem.instance.currentFolder.files)
        {
            output += "\n -" + file.getName();
        }
    }

    public bool ParameterVoid(string parameter)
    {
        return (parameter == "");
    }
}

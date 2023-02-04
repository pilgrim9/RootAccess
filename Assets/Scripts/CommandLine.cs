using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CommandLine : MonoBehaviour
{
    public static CommandLine instance;

    public event Action<string, string, string> onCommand;

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
        theMethod.Invoke(this, new[] { parameter });
        onCommand.Invoke(command, FileSystem.instance.currentFolder.FolderPath, parameter);
        return getOutput(input);
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
            output = "Error: Please specify a file to download.";
            OutputDownloadables();
            return;
        }

        if (!MailMission.instance.apps.Contains(parameter))
        {
            output = "Error: That is not a file you can download.";
            OutputDownloadables();
            return;
        }

        if (FileSystem.instance.currentFolder.ContainsFile(parameter))
        {
            output = "Error: You already downloaded this file.";
            return;
        }
        FileSystem.instance.currentFolder.Add(new OSFile(parameter));
    }
    public void OutputDownloadables()
    {
        output += "\n You can download these files:\n";
        foreach (var name in MailMission.instance.apps)
        {
            output += "\n"+ name ;
        }
    }
    public void Install(string parameter)
    {
        if (!MailMission.instance.apps.Contains(parameter))
        {
            output = "You need to install you have downloaded."; 
            OutputDownloadables();
            return;
        }
        if (!FileSystem.instance.currentFolder.ContainsFile(parameter))
        {
            output = "The file doesn't exist. You may need to download it first.";
            return;
        }
        
        FileSystem.instance.currentFolder.Cut(parameter);
        output = parameter + " app installed successfully!";
    }
    
    public void Cut(string file)
    {
        if (!FileSystem.instance.currentFolder.ContainsFile(file))
        {
            output = "File does not exist in this folder";
            return;
        }

        if (FileSystem.instance.clipboard != null && FileSystem.instance.clipboard.name !="")
        {
            output = "You cannot cut while holding another file. Paste first please.";
            return;
        }
        FileSystem.instance.clipboard = FileSystem.instance.currentFolder.Cut(file);
        output = "Cut file " + file;
        List("");
    }
    public void Paste(string parameters)
    {
        Debug.Log(FileSystem.instance.clipboard);
        if (FileSystem.instance.clipboard != null && FileSystem.instance.clipboard.name !="")
        {
            FileSystem.instance.currentFolder.Add(FileSystem.instance.clipboard);
            FileSystem.instance.clipboard = null;
            output = "File Pasted";
            List("");
        }
        else
        {
            output = "Nothing to paste. You need to cut a file first.";
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
        ListFolders();
        ListFiles();
    }

    public void ListFolders()
    {
        output += "This folder contains "+FileSystem.instance.currentFolder.subfolders.Count+" folders:";
        foreach (var folder in FileSystem.instance.currentFolder.subfolders)
        {
            output += "\n -<color="+ Colors.FolderColor +">" + folder.getName() + "</color>";
        }
    }

    public void ListFiles()
    {
        output += "\nThis folder contains "+FileSystem.instance.currentFolder.files.Count+" files:";
        foreach (var file in FileSystem.instance.currentFolder.files)
        {
            output += "\n -<color="+ Colors.FileColor +">" + file.getName() + "</color>";
        }
    }
    
    bool ParameterVoid(string parameter)
    {
        return (parameter == "");
    }
}

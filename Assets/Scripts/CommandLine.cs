using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

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
            Error("Demasiados argumentos");
            return getOutput(input);
        }
        
        string command = parameters[0].ToLower();
        string parameter = parameters.Length == 1? "" : parameters[1].ToLower();

        if (!commands.Keys.Contains(command))
        {
            Error("El comando " + command + " no existe.");
            return getOutput(input);
        }
        invokeCommand(command, parameter);
        return getOutput(input);
    }

    private void invokeCommand(string command, string parameter)
    {
        Type thisType = this.GetType();
        MethodInfo theMethod = thisType.GetMethod(commands[command]);
        theMethod.Invoke(this, new[] { parameter });
        onCommand.Invoke(command, FileSystem.instance.currentFolder.FolderPath, parameter);
    }

    private string getOutput(string input)
    {
        string newOutput = "\n" + "Carpeta actual:/" +  GetPath() + " >" + input + "\n" + output;
        return colorCommands(newOutput);
    }

    private string GetPath()
    {
        // returns <color=#folder>folder name</color>/<color=#folder>folder2 name </color>...
        return "<color=" + Colors.FolderColor + ">" +
               FileSystem.instance.currentFolder.FolderPath.Replace("/", "</color>/<color=" + Colors.FolderColor + ">")
               + "</color>/";
    }

    public string colorCommands(string input)
    {
        return Colors.ReplaceCollection(input, commands.Keys, Colors.CommandColor);
    }

    private void clearOutput()
    {
        output = "";
    }

    public void Open(string parameter)
    {
        if (ParameterVoid(parameter))
        {
            Error("Por favor, ingresa el nombre de la carpeta");
            return;
        }

        if (!FileSystem.instance.currentFolder.ContainsFolder(parameter))
        {
            Error("La carpeta \"" + parameter + "\" no existe en "+ FileSystem.instance.currentFolder.getName());
            return;
        }

        OSFolder folder = FileSystem.instance.currentFolder.GetFolder(parameter);
        FileSystem.instance.currentFolder = folder;
        List("");
    }

    public void Download(string parameter)
    {
        if (ParameterVoid(parameter))
        {
            Error("Por favor, especifique el nombre del archivo.");
            OutputDownloadables();
            return;
        }

        if (!MailMission.instance.apps.Contains(parameter, StringComparer.OrdinalIgnoreCase))
        {
            Error(parameter + " no es un archivo descargable.");
            OutputDownloadables();
            return;
        }

        if (FileSystem.instance.currentFolder.ContainsFile(parameter))
        {
            Error("Archivo ya descargado.");
            return;
        }
        FileSystem.instance.currentFolder.Add(new OSFile(parameter));
        output = parameter + " descarga completada.\n";
        List("");
    }
    public void OutputDownloadables()
    {
        output += "\nPuedes descargar las siguientes aplicaciones:";
        foreach (var name in MailMission.instance.apps)
        {
            output += "\n"+ Colors.File(name) ;
        }
    }
    public void Install(string parameter)
    {
        if (!MailMission.instance.apps.Contains(parameter, StringComparer.OrdinalIgnoreCase))
        {
            Error("Unicamente puedes instalar un archivo descargado."); 
            OutputDownloadables();
            return;
        }
        if (!FileSystem.instance.currentFolder.ContainsFile(parameter))
        {
            Error("El archivo no existe, puede que necesites descargarlo primero.");
            return;
        }
        
        FileSystem.instance.currentFolder.Cut(parameter);
        output ="Aplicacion "+Colors.File(parameter)+" instalada correctamente!";
    }
    
    public void Cut(string file)
    {
        if (!FileSystem.instance.currentFolder.ContainsFile(file))
        {
            Error("El archivo no existe en esta carpeta");
            return;
        }

        if (FileSystem.instance.clipboard != null && FileSystem.instance.clipboard.name !="")
        {
            Error("No puedes cortar un archivo hasta pegues el anterior.");
            return;
        }
        FileSystem.instance.clipboard = FileSystem.instance.currentFolder.Cut(file);
        output = "Archivo cortado: "+Colors.File(name)+".\n";
        List("");
    }
    public void Paste(string parameters)
    {
        Debug.Log(FileSystem.instance.clipboard);
        if (FileSystem.instance.clipboard != null && FileSystem.instance.clipboard.name !="")
        {
            FileSystem.instance.currentFolder.Add(FileSystem.instance.clipboard);
            FileSystem.instance.clipboard = null;
            output = "Archivo pegado.\n";
            List("");
        }
        else
        {
            Error("Primero debes cortar un archivo.");
        }
    }
    public void Back(string parameter)
    {
        if (!ParameterVoid(parameter))
        {
            Error("Back no requiere parametros.");
            return;
        }

        if (FileSystem.instance.currentFolder.ParentFolder == null || FileSystem.instance.currentFolder.ParentFolder.getName() == "")
        {
            Error("No puedes ir mas hacia atras.");
            return;
        }
        FileSystem.instance.currentFolder = FileSystem.instance.currentFolder.ParentFolder;
        List("");
    }

   
    public void List(string parameter)
    {
        if (!ParameterVoid(parameter))
        {
            Error("List no requiere parametros.");
            return;
        }
        output += "Contenidos de la carpeta:";
        ListFolders();
        ListFiles();
    }

    public void ListFolders()
    {
        foreach (var folder in FileSystem.instance.currentFolder.subfolders)
        {
            output += " -"+Colors.Folder(folder.getName()) + "\n";
        }
    }

    public void ListFiles()
    {
        foreach (var file in FileSystem.instance.currentFolder.files)
        {
            output += " -"+Colors.File(file.getName()) + "\n";
        }
    }
    
    bool ParameterVoid(string parameter)
    {
        return (parameter == "");
    }

    void Error(string errorString)
    {
        output = Colors.Error("Error")+ ": " + errorString;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
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
        Debug.Log(input);
        clearOutput();
        string[] parameters = input.Split(" ");
        if (parameters.Length > 2)
        {
            output = "Contiene muchos argumentos";
            return getOutput(input);
        }
        Debug.Log(parameters);
        
        string command = parameters[0].ToLower();
        string parameter = parameters.Length == 1? "" : parameters[1].ToLower();

        if (!commands.Keys.Contains(command))
        {
            output = "Comando "+command+" no existe";
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
        string newOutput = "\n" + "Carpeta actual:" + 
                 "<color=" + Colors.FileColor + ">" +
                 FileSystem.instance.currentFolder.FolderPath.Replace("\\","</color>\\<color=" + Colors.FileColor + ">")
                 + "</color>> " 
                 + input + "\n" + output;
        return colorCommands(newOutput);
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
            output = "Por favor, ingresa el nombre de la carpeta";
            return;
        }

        Debug.Log(parameter);
        if (!FileSystem.instance.currentFolder.ContainsFolder(parameter))
        {
            output = "La carpeta \"" + parameter + "\" no existe en la carpeta en la que te encuentras";
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
            output = "Error: Porfavor, especifique el nombre del archivo.";
            OutputDownloadables();
            return;
        }

        if (!MailMission.instance.apps.Contains(parameter, StringComparer.OrdinalIgnoreCase))
        {
            output = "Error: No es un archivo descargable.";
            OutputDownloadables();
            return;
        }

        if (FileSystem.instance.currentFolder.ContainsFile(parameter))
        {
            output = "Error: Archivo ya descargado.";
            return;
        }
        FileSystem.instance.currentFolder.Add(new OSFile(parameter));
        output = parameter + " descarga completada";
        List("");
    }
    public void OutputDownloadables()
    {
        output += "\nPuedes descargar las siguientes aplicaciones:";
        foreach (var name in MailMission.instance.apps)
        {
            output += "\n"+ name ;
        }
    }
    public void Install(string parameter)
    {
        if (!MailMission.instance.apps.Contains(parameter, StringComparer.OrdinalIgnoreCase))
        {
            output = "Unicamente puedes instalar un archivo descargado."; 
            OutputDownloadables();
            return;
        }
        if (!FileSystem.instance.currentFolder.ContainsFile(parameter))
        {
            output = "El archivo no existe, puede que necesites descargarlo primero.";
            return;
        }
        
        FileSystem.instance.currentFolder.Cut(parameter);
        output = parameter + " aplicacion instalada correctamente!";
    }
    
    public void Cut(string file)
    {
        if (!FileSystem.instance.currentFolder.ContainsFile(file))
        {
            output = "El archivo no existe en esta carpeta";
            return;
        }

        if (FileSystem.instance.clipboard != null && FileSystem.instance.clipboard.name !="")
        {
            output = "No puedes cortar un archivo hasta pegues el anterior.";
            return;
        }
        FileSystem.instance.clipboard = FileSystem.instance.currentFolder.Cut(file);
        output = "Archivo cortado " + file;
        List("");
    }
    public void Paste(string parameters)
    {
        Debug.Log(FileSystem.instance.clipboard);
        if (FileSystem.instance.clipboard != null && FileSystem.instance.clipboard.name !="")
        {
            FileSystem.instance.currentFolder.Add(FileSystem.instance.clipboard);
            FileSystem.instance.clipboard = null;
            output = "Archivo pegado";
            List("");
        }
        else
        {
            output = "Error, primero debes cortar un archivo.";
        }
    }
    public void Back(string parameter)
    {
        if (!ParameterVoid(parameter))
        {
            output = "Back no requiere parametros!";
            return;
        }

        if (FileSystem.instance.currentFolder.ParentFolder == null || FileSystem.instance.currentFolder.ParentFolder.getName() == "")
        {
            output = "No puedes ir mas hacia atras";
            return;
        }
        FileSystem.instance.currentFolder = FileSystem.instance.currentFolder.ParentFolder;
        List("");
    }

   
    public void List(string parameter)
    {
        if (!ParameterVoid(parameter))
        {
            output = "List no requiere parametros!";
            return;
        }
        output += "\n";
        ListFolders();
        ListFiles();
    }

    public void ListFolders()
    {
        OSFolder current = FileSystem.instance.currentFolder;
        output += "Esta carpeta contiene "+FileSystem.instance.currentFolder.subfolders.Count+" carpetas:";
        foreach (var folder in FileSystem.instance.currentFolder.subfolders)
        {
            output += "\n -<color="+ Colors.FolderColor +">" + folder.getName() + "</color>";
        }
    }

    public void ListFiles()
    {
        output += "\nEsta carpeta contiene "+FileSystem.instance.currentFolder.files.Count+" archivos:";
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

using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using Object = System.Object;

[System.Serializable]
public class OSFolder: Object
{
    public OSFolder(){}
    public OSFolder(string name, OSFolder parent=null)
    {
        this.name = name;        
        subfolders = new List<OSFolder>();
        files = new List<OSFile>();
        BuildFolderPath();
        ParentFolder = parent;
    }

    public void BuildFolderPath()
    {
        FolderPath = "\\" +  (ParentFolder != null? ParentFolder.FolderPath + "\\" + name : name);
    }

    public string getName()
    {
        return name.ToLower();
    }
    public string name;
    
    public List<OSFolder> subfolders = new List<OSFolder>();
    public List<OSFile> files = new List<OSFile>();
    [NonSerialized] public string FolderPath;
    [NonSerialized] public OSFolder ParentFolder;


    public bool ContainsFolder(string FolderName)
    {
        foreach (var folder in subfolders)
        {
            if (folder.getName() == FolderName)
            {
                return true;
            }
        }
        return false;   
    }

    public bool ContainsFile(string fileName)
    {
        foreach (var file in files)
        {
            if (file.getName() == fileName)
            {
                return true;
            }
        }
        return false;
    }
    public bool Contains(string fileName)
    {
        return (ContainsFolder(fileName) || ContainsFile(fileName));
    }

    public OSFolder GetFolder(string folderName)
    {
        foreach (var folder in subfolders)
        {
            if (folder.getName() == folderName)
            {
                return folder;
            }
        }

        return null;
    } 

    public Object Get(string fileName)
    {
        foreach (var folder in subfolders)
        {
            if (folder.getName() == fileName)
            {
                return folder;
            }
        }

        foreach (var file in files)
        {
            if (file.getName() == fileName)
            {
                return file;
            }
        }

        return false;
    }

    public OSFile Cut(string fileName)
    {
        foreach (var file in  files)
        {
            if (file.getName() == fileName)
            {
                files.Remove(file);
                return file;
            }
        }
        return null;
    }

    public void Add(Object file)
    {
        if (file.GetType() == typeof(OSFolder))
        {
            subfolders.Add((OSFolder)file);
        }
        else
        {
            files.Add((OSFile)file);
        }
    }
}

[System.Serializable]
public class OSFile : Object
{
    public string getName()
    {
        return name.ToLower();
    }
    public string name;
    [NonSerialized] public string targetFolderPath;
}


using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
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
        FolderPath = ParentFolder != null? ParentFolder.FolderPath: "root:"+"/"+name;
    }
    public string name;
    public List<OSFolder> subfolders;
    public List<OSFile> files;
    [NonSerialized] public string FolderPath;
    [NonSerialized] public OSFolder ParentFolder;


    public bool Contains(string fileName)
    {
        foreach (var folder in subfolders)
        {
            if (folder.name == fileName)
            {
                return true;
            }
        }

        foreach (var file in files)
        {
            if (file.name == fileName)
            {
                return true;
            }
        }

        return false;
    }
    public Object Get(string fileName)
    {
        foreach (var folder in subfolders)
        {
            if (folder.name == fileName)
            {
                return folder;
            }
        }

        foreach (var file in files)
        {
            if (file.name == fileName)
            {
                return file;
            }
        }

        return false;
    }

    public Object Cut(string fileName)
    {
        foreach (var file in  files)
        {
            if (file.name == fileName)
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
    public string name;
    [NonSerialized] public string targetFolderPath;
}


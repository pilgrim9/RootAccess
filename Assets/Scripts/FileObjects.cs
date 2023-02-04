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
    public OSFolder(string path, string name)
    {
        folderPath = path+"/"+name;
        this.name = name;
        subfolders = new List<OSFolder>();
        files = new List<OSFile>();
    }
    [NonSerialized] public string folderPath;
    [NonSerialized] public OSFolder ParentFolder;
    public string name;
    public List<OSFolder> subfolders;
    public List<OSFile> files;

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
}

[System.Serializable]
public class OSFile : Object
{
    public string name;
    [NonSerialized] public string targetFolderPath;
}


using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using Object = System.Object;

[System.Serializable]
public class OSFolder: Object
{
    public OSFolder(){}
    public OSFolder(string path, string name)
    {
        folderPath = path+"/"+name;
        folderName = name;
        subfolders = new List<OSFolder>();
        files = new List<OSFile>();
    }
    [NonSerialized] public string folderPath;
    [NonSerialized] public OSFolder ParentFolder;
    public string folderName;
    public List<OSFolder> subfolders;
    public List<OSFile> files;
   
}

[System.Serializable]
public class OSFile : Object
{
    public string fileName;
    public string targetFolderPath;
}


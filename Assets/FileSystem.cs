using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class FileSystem : MonoBehaviour
{
    private OSFolder root;
    private List<string> users = new List<string>()
    {
        "1",
        "2",
        "3",
        "4"
    };
    
    private List<string> folders = new List<string>()
    {
        "documentos",
        "fotos",
    };
    
    private void init()
    {
        root = new OSFolder("C:", "");
        createUsers(root, users);
    }

    private void createUsers(OSFolder parentFolder, List<string> files)
    {
        foreach (var file in files)
        {
            OSFolder userFolder = new OSFolder(parentFolder.folderPath + file, file);
            createDocumentFolders(userFolder, folders);
            parentFolder.subfolders.Add(userFolder);
        }
        
    }
    
    private void createDocumentFolders(OSFolder parentFolder, List<string> files)
    {
        foreach (var file in files)
        {
            OSFolder documentFolder = new OSFolder(parentFolder.folderPath, file);
            parentFolder.subfolders.Add(documentFolder);
        }
        
    }
}

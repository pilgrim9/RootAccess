using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class FileSystem : MonoBehaviour
{
    public Object clipboard;
    public OSFolder currentFolder;
    private OSFolder root;
    public static FileSystem instance;
    public UserSO[] users;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        root = new OSFolder("");
        CreateUsers(root);
        currentFolder = root;
    }

    private void CreateUsers(OSFolder parentFolder)
    {
        foreach (var user in users)
        {
            OSFolder userFolder = new OSFolder(user.Name, parentFolder);
            userFolder.subfolders = user.folders;
            userFolder.files = user.files;
            parentFolder.subfolders.Add(userFolder);
            foreach (var folder in userFolder.subfolders)
            {
                folder.ParentFolder = userFolder;
                folder.BuildFolderPath();
            }
        }
    }
}

using UnityEngine;

public class FileSystem : MonoBehaviour
{
    public OSFile clipboard;
    public OSFolder currentFolder;
    public OSFolder root;
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
        root = new OSFolder("root");
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
            ParentThisFolder(userFolder);
        }
    }

    public void ParentThisFolder(OSFolder parentFolder)
    {
        foreach (var folder in parentFolder.subfolders)
        {
            folder.ParentFolder = parentFolder;
            folder.BuildFolderPath();
            if (folder.subfolders.Count > 0)
            {
                ParentThisFolder(folder);
            } 
        }
    }

}

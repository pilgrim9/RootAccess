using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class ActiveMission
{
    string file, user, folder;
    MissionType missionType;

    public ActiveMission(string file, string user, string folder, MissionType missionType)
    {
        this.file = file;
        this.user = user;
        this.folder = folder;
        this.missionType = missionType;
    }

    public bool isMissionComplete(string command, string place, string file)
    {
        Debug.Log(this.user);
        Debug.Log(this.folder);
        Debug.Log(this.missionType);
        Debug.Log(this.file);        
        Debug.Log(command);
        Debug.Log(place);
        Debug.Log(file);
        Debug.Log(command == "install");
        Debug.Log(file == this.file);



        if (missionType == MissionType.Install) { 
            if (command.ToLower() == "install" && file.ToLower() == this.file.ToLower() && place.Contains(user, System.StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log("win");
                return true;
            }
        }
        if (missionType == MissionType.Move) {
            Debug.Log(FileSystem.instance.currentFolder.ContainsFile(this.file));
            Debug.Log(FileSystem.instance.currentFolder.name);
            Debug.Log(this.file);
            Debug.Log(command);
            Debug.Log(place);
            Debug.Log(file);

            if (command.ToLower() == "paste" && place.Contains(user, System.StringComparison.OrdinalIgnoreCase) && place.Contains(folder, System.StringComparison.OrdinalIgnoreCase ) && FileSystem.instance.currentFolder.ContainsFile(this.file)) 
            {
                Debug.Log("ganaste");
                return true;              

            }
        }
        return false;
    }
}

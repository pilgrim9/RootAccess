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
        this.file= file;
        this.user = user;
        this.folder = folder;
        this.missionType = missionType;
    }

    public bool isMissionComplete(string command, string place, string file)
    {
        Debug.Log(command + " " + place + " " + file + " " + missionType);

        if (missionType == MissionType.Install) { 
            if (command == "install" && file == this.file && place.Contains(user))
            {
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

            if (command == "paste" && place.Contains(user) && place.Contains(folder) && FileSystem.instance.currentFolder.ContainsFile(this.file)) 
            {
                Debug.Log("ganaste");
                return true;              

            }
        }
        return false;
    }
}

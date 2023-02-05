using System.Collections;
using System.Collections.Generic;
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
        if (missionType == MissionType.Install) { 
            if (command == "install" && file == this.file && place.Contains(user))
            {
                return true;
            }
        }
        if (missionType == MissionType.Move) { 
            if (command == "paste" && file == this.file && place.Contains(user) && place.Contains(folder))
            {
                return true;

            }
        }
        return false;
    }
}

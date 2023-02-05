using System;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class ActiveMission
{
    string file, user, folder;
    MissionType missionType;
    public bool isTutorial = false;

    public ActiveMission(string file, string user, string folder, MissionType missionType)
    {
        this.file = file;
        this.user = user;
        this.folder = folder;
        this.missionType = missionType;
    }

    public virtual bool isMissionComplete(string command, string place, string file)
    {
        if (missionType == MissionType.Install) { 
            if (command.ToLower() == "install" && file.ToLower() == this.file.ToLower() && place.Contains(user, StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log("win");
                return true;
            }
        }
        if (missionType == MissionType.Move) {

            if (command.ToLower() == "paste" && place.Contains(user, StringComparison.OrdinalIgnoreCase) && place.Contains(folder, StringComparison.OrdinalIgnoreCase ) && FileSystem.instance.currentFolder.ContainsFile(this.file)) 
            {
                Debug.Log("ganaste");
                return true;              

            }
        }
        return false;
    }
}

public class TutorialMission : ActiveMission
{
    string file, user, folder, command;
    MissionType missionType;
    public TutorialMission nextMission;
    public Action callback;
    public TutorialMission(string file, string user, string folder, MissionType missionType, string command): base (file, user, folder, missionType)
    {
        this.command = command;
        this.file = file;
        this.user = user; // unused
        this.folder = folder;
        this.missionType = missionType; // unused
        isTutorial = true;
    }

    public override bool isMissionComplete(string sentCommand, string place, string parameterFile)
    {
        if (sentCommand.ToLower() != this.command.ToLower()) return false;
        if (this.file != "")
        {
            if (parameterFile.ToLower() != this.file.ToLower()) return false;
        }
        if (folder != "")
        {
            if (FileSystem.instance.currentFolder.getName() != this.folder) return false;
        }
        return true;
    }
}

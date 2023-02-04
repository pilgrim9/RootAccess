using System.Collections.Generic;
using UnityEngine;

public class MailMission : MonoBehaviour

{
    public MissionSO install, move;

    public List<string> files, apps, firstFileName, secondFileName, formatName, filesCreates;

    UserSO[] users;

    void Start()
    {
        users = FileSystem.instance.users;

    }

    MissionSO SelectMission()
    {
        int randomType = Random.Range(0, 3);
        if (randomType == 0) return move;
        else return install;
    }

    string CreateMission()
    {
        MissionSO newMission = SelectMission();

        string mission = newMission.text;

        int randomUser = Random.Range(0, users.Length);
        mission = mission.Replace("{player}", users[randomUser].Name);
        string file = CreateFile();

        if (newMission.type == 0)
        {
            int randomFolder = Random.Range(0, users[randomUser].folders.Count);
            mission = mission.Replace("{folder}", users[randomUser].folders[randomFolder].name);

            mission = mission.Replace("{file}", file );
            AddFile(users[randomUser], file, users[randomUser].folders[randomFolder].name);
        }
        else
        {
            int randomApp = Random.Range(0, apps.Count);
            mission = mission.Replace("{app}", apps[randomApp]);
        }

        return mission;
    }

    string CreateFile()
    {
        int randomFirst = Random.Range(0, firstFileName.Count);
        int randomSecond = Random.Range(0, secondFileName.Count);
        int randomFormat = Random.Range(0, formatName.Count);
        string file = firstFileName[randomFirst] +  secondFileName[randomSecond] + Random.Range(1, 10) + formatName[randomFormat];

        while(filesCreates.Contains(file))
        {
             randomFirst = Random.Range(0, firstFileName.Count);
             randomSecond = Random.Range(0, secondFileName.Count);

            file = firstFileName[randomFirst] + " " + secondFileName[randomSecond] + Random.Range(1, 1000);
        }
         return file;
    }

    void AddFile(UserSO user, string file, string folder) 
    {
        OSFile oSFile = new OSFile();
        oSFile.name = file;
        
        
        int isFilesInFolderMother = Random.Range(0, 4);

        if (isFilesInFolderMother == 0) user.files.Add(oSFile);

        else {
            int randomFolder = Random.Range(0, user.folders.Count);

            if (user.folders[randomFolder].subfolders.Count == 0 && user.folders[randomFolder].name != folder) user.folders[randomFolder].files.Add(oSFile);
        else if (user.folders[randomFolder].subfolders.Count == 0)
        {
            AddFile(user,file,folder);
            return;
        }
        else
        {
            int folderOrSubfolder  = Random.Range(0, 3);
            if(folderOrSubfolder == 0) user.folders[randomFolder].files.Add(oSFile);
            else
            {
                int randomSubfolder = Random.Range(0, user.folders[randomFolder].subfolders.Count);
                user.folders[randomFolder].subfolders[randomSubfolder].Add(oSFile);
            }
        }
        }
    }
    private void Update()
    {
       if( Input.GetKeyDown("w")) print(CreateMission());
    }
}
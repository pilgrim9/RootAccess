using System.Collections.Generic;
using UnityEngine;

public class MailMission : MonoBehaviour

{
    public MissionSO install, move;

    public List<string> files, apps, firstFileName, secondFileName, filesCreates;

    UserSO[] users;

    void Start()
    {
        users = Resources.LoadAll<UserSO>("UsersSO");
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

        int randomUser = Random.Range(0, users.Length);
        newMission.text.Replace("{player}", users[randomUser].Name);
        string file = CreateFile();

        if (newMission.type == 0)
        {

            int randomFolder = Random.Range(0, users[randomUser].folders.Count);
            newMission.text.Replace("{folder}", users[randomUser].folders[randomFolder].name);

            newMission.text.Replace("{file}", file );
        }
        else
        {
            int randomApp = Random.Range(0, apps.Count);
            newMission.text.Replace("{app}", apps[randomApp]);
        }

        AddFile(users[randomUser], file);

        return newMission.text;
    }

    string CreateFile()
    {
        int randomFirst = Random.Range(0, firstFileName.Count);
        int randomSecond = Random.Range(0, secondFileName.Count);

        string file = firstFileName[randomFirst] + " " + secondFileName[randomSecond] + Random.Range(1, 1000);

        while(filesCreates.Contains(file))
        {
             randomFirst = Random.Range(0, firstFileName.Count);
             randomSecond = Random.Range(0, secondFileName.Count);

            file = firstFileName[randomFirst] + " " + secondFileName[randomSecond] + Random.Range(1, 1000);
        }
         return file;
    }

    void AddFile(UserSO user, string file) 
    {
        OSFile oSFile = new OSFile();
        oSFile.name = file;

        int randomFolder = Random.Range(0, user.folders.Count);
        if (user.folders[randomFolder].subfolders.Count == 0) user.folders[randomFolder].files.Add(oSFile);
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
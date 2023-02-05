using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;
using System;
using Random = UnityEngine.Random;
using TMPro;

public class MailMission : MonoBehaviour

{
    public MissionSO[] missions;
    public static MailMission instance;

    public List<string> files, apps, firstFileName,  formatName, filesCreates;

    public ActiveMission currentMission;
    public UnityEvent onMissionComplete;
    public event Action onMissionCompleteAction;
    public UnityEvent<string> onMissionCreated;

    public TextMeshProUGUI emailText;


    public string playerName;
    private void Awake()
    {
        instance = this;
        playerName = PlayerPrefs.GetString("Name");
    }
    
    void Start()
    {
        missions = Resources.LoadAll<MissionSO>("MisssionSO");
        CommandLine.instance.onCommand += OnCommand;
    }

    MissionSO SelectMission()
    {
        int randomType = Random.Range(0, missions.Length);
        return missions[randomType];
    }

    public void OnCommand(string command, string place, string file)
    {
        if (currentMission.isMissionComplete(command, place, file))
        {
            onMissionCompleteAction?.Invoke();
            onMissionComplete.Invoke();
            CreateMission();
        }
    }
    private UserSO[] users { get
        {
            return FileSystem.instance.users;
        }
    }

    public void StartFirstMission(MissionSO firstMission)
    {
        string missionText = firstMission.text;
        currentMission = new ActiveMission("TioBorracho.png", "matias", "Navidad", MissionType.Move);
        onMissionCreated.Invoke(missionText);
        emailText.text = missionText;
        
    }

    public void CreateMission()
    {
        MissionSO missionTemplate = SelectMission();

        string missionText = missionTemplate.text;

        int randomUser = Random.Range(0, users.Length);
        missionText = missionText.Replace("{user}", users[randomUser].Name);
        missionText = missionText.Replace("{player}", playerName);

        OSFolder userFolder = FileSystem.instance.root.GetFolder(users[randomUser].Name);
        if (missionTemplate.type == MissionType.Move)
        {
            string file = CreateFile();
            int randomFolder = Random.Range(0, users[randomUser].folders.Count);
            missionText = missionText.Replace("{folder}", userFolder.subfolders[randomFolder].getName());
            missionText = missionText.Replace("{file}", file);
            AddFile(userFolder, file, userFolder.subfolders[randomFolder].getName());
            currentMission = new ActiveMission(file, users[randomUser].Name, userFolder.subfolders[randomFolder].getName(), missionTemplate.type);
        }
        else
        {
            int randomApp = Random.Range(0, apps.Count);
            missionText = missionText.Replace("{app}", apps[randomApp]);
            currentMission = new ActiveMission(apps[randomApp], users[randomUser].Name, "", missionTemplate.type);

        }
        
        onMissionCreated.Invoke(missionText);
        emailText.text = missionText;

    }

    string CreateFile()
    {
        int randomFirst = Random.Range(0, firstFileName.Count);
        int randomFormat = Random.Range(0, formatName.Count);
        string file = firstFileName[randomFirst] +  Random.Range(1, 100) + formatName[randomFormat];

        while (filesCreates.Contains(file))
        {
            randomFirst = Random.Range(0, firstFileName.Count);

            file = firstFileName[randomFirst] + Random.Range(1, 100) + formatName[randomFormat];
        }
        return file;
    }

    void AddFile(OSFolder folderObject, string file, string folder)
    {
        OSFile oSFile = new OSFile();
        oSFile.name = file;


        int isFilesInFolderMother = Random.Range(0, 4);

        if (isFilesInFolderMother == 0) folderObject.files.Add(oSFile);

        else
        {
            int randomFolder = Random.Range(0, folderObject.subfolders.Count);

            if (folderObject.subfolders[randomFolder].subfolders.Count == 0 && folderObject.subfolders[randomFolder].getName() != folder) folderObject.subfolders[randomFolder].files.Add(oSFile);
            else if (folderObject.subfolders[randomFolder].subfolders.Count == 0)
            {
                AddFile(folderObject, file, folder);
                return;
            }
            else
            {
                int folderOrSubfolder = Random.Range(0, 3);
                if (folderOrSubfolder == 0) folderObject.subfolders[randomFolder].files.Add(oSFile);
                else
                {
                    int randomSubfolder = Random.Range(0, folderObject.subfolders[randomFolder].subfolders.Count);
                    folderObject.subfolders[randomFolder].subfolders[randomSubfolder].Add(oSFile);
                }
            }
        }
    }
}
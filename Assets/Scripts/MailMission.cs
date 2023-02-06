using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;
using System;
using Random = UnityEngine.Random;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.PlayerLoop;

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
    public MissionSO[] tutorialMissions;
    public TMP_Text portapapeles; 


    public string playerName;
    private void Awake()
    {
        instance = this;
        playerName = PlayerPrefs.GetString("Name");
        missions = Resources.LoadAll<MissionSO>("MisssionSO");
    }
    
    void Start()
    {
        CommandLine.instance.onCommand += OnCommand;
    }

    private void Update()
    {
        if (FileSystem.instance.clipboard == null)
        {
            portapapeles.text = "";
        }
        else
        {
            portapapeles.text = FileSystem.instance.clipboard.getName();
        }
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
            if (currentMission.isTutorial)
            {
                TutorialMission mission = (TutorialMission)currentMission;
                mission.callback.Invoke();
            }
            else
            {
                CreateMission();
            }
        }
    }
    private UserSO[] users { get
        {
            return FileSystem.instance.users;
        }
    }

    public void StartFirstMission()
    {
        string missionText = tutorialMissions[0].text;
        TutorialMission mission = new TutorialMission("", "", "matias", 0, "open");
        
        mission.callback += StartSecondMission;
        currentMission = mission;
        onMissionCreated.Invoke(missionText);
        updateEmailText(missionText);
    }
    public void StartSecondMission()
    {
        string missionText = tutorialMissions[1].text;
        TutorialMission mission = new TutorialMission("", "", "", 0, "back");
        
        mission.callback += StartThirdMission;
        currentMission = mission;
        onMissionCreated.Invoke(missionText);
        updateEmailText(missionText);
    }
    public void StartThirdMission()
    {
        string missionText = tutorialMissions[2].text;
        TutorialMission mission = new TutorialMission("photoshop", "", "", 0, "download");
        
        mission.callback += StartFourthMission;
        currentMission = mission;
        onMissionCreated.Invoke(missionText);
        updateEmailText(missionText);
    }public void StartFourthMission()
    {
        string missionText = tutorialMissions[3].text;
        TutorialMission mission = new TutorialMission("photoshop", "", "", 0, "install");
        
        mission.callback += StartFifthMission;
        currentMission = mission;
        onMissionCreated.Invoke(missionText);
        updateEmailText(missionText);
    }public void StartFifthMission()
    {
        string missionText = tutorialMissions[4].text;
        TutorialMission mission = new TutorialMission("", "", "trabajo", 0, "open");
        
        mission.callback += StartSixthMission;
        currentMission = mission;
        onMissionCreated.Invoke(missionText);
        updateEmailText(missionText);
    }public void StartSixthMission()
    {
        string missionText = tutorialMissions[5].text;
        TutorialMission mission = new TutorialMission("", "", "navidad", 0, "paste");
        
        mission.callback += StartSeventhMission;
        currentMission = mission;
        onMissionCreated.Invoke(missionText);
        updateEmailText(missionText);
    }
    void callEnd()
    {
        GameManager.instance.endGame(); 
    }
    public void StartSeventhMission()
    {
        string missionText = tutorialMissions[6].text;
        // no callback
        
        Invoke(nameof(callEnd), 12);
        onMissionCreated.Invoke(missionText);
        updateEmailText(missionText);
    }

    public void updateEmailText(string missionText)
    {
        emailText.text = CommandLine.instance.colorCommands(missionText);
    }
    public void CreateMission()
    {
        MissionSO missionTemplate = SelectMission();

        string missionText = missionTemplate.text;

        int randomUser = Random.Range(0, users.Length);
        missionText = missionText.Replace("{user}", Colors.Wrap(users[randomUser].Name, Colors.FolderColor));
        missionText = missionText.Replace("{player}", playerName);

        OSFolder userFolder = FileSystem.instance.root.GetFolder(users[randomUser].Name);
        if (missionTemplate.type == MissionType.Move)
        {
            string file = CreateFile();
            int randomFolder = Random.Range(0, users[randomUser].folders.Count);
            missionText = missionText.Replace("{folder}", Colors.Wrap(userFolder.subfolders[randomFolder].getName(), Colors.FolderColor));
            missionText = missionText.Replace("{file}", Colors.Wrap(file, Colors.FileColor));
            AddFile(userFolder, file, userFolder.subfolders[randomFolder].getName());
            currentMission = new ActiveMission(file, users[randomUser].Name, userFolder.subfolders[randomFolder].getName(), missionTemplate.type);
        }
        else
        {
            int randomApp = Random.Range(0, apps.Count);
            missionText = missionText.Replace("{app}", Colors.Wrap(apps[randomApp], Colors.FileColor));
            currentMission = new ActiveMission(apps[randomApp], users[randomUser].Name, "", missionTemplate.type);

        }
        
        onMissionCreated.Invoke(missionText);
        updateEmailText(missionText);

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
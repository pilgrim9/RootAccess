using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;
using TMPro;

public class MailMission : MonoBehaviour

{
    public MissionSO[] missions;
    public static MailMission instance;

    public List<string> files, apps, firstFileName,  formatName, filesCreates;

    public ActiveMission currentMission;
    public UnityEvent onMissionComplete;
    public UnityEvent<string> onMissionCreated;

    public TextMeshProUGUI textMails;
    public AudioSource audioSource;
    public AudioClip audioClip;

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
            onMissionComplete.Invoke();
            CreateMission(SelectMission());
        }
    }
    private UserSO[] users { get
        {
            return FileSystem.instance.users;
        }
    }

    public void CreateRandomMission()
    {
        CreateMission(SelectMission());
    }

    public void CreateMission(MissionSO missionTemplate)
    {
        audioSource.PlayOneShot(audioClip);
        string missionText = missionTemplate.text;

        int randomUser = Random.Range(0, users.Length);
        missionText = missionText.Replace("{user}", users[randomUser].Name);
        missionText = missionText.Replace("{player}", playerName);
        string file = CreateFile();

        OSFolder userFolder = FileSystem.instance.root.GetFolder(users[randomUser].name);
        if (missionTemplate.type == MissionType.Move)
        {
            int randomFolder = Random.Range(0, users[randomUser].folders.Count);
            missionText = missionText.Replace("{folder}", userFolder.subfolders[randomFolder].name);
            missionText = missionText.Replace("{file}", file);
            AddFile(userFolder, file, userFolder.subfolders[randomFolder].name);
            currentMission = new ActiveMission(file, users[randomUser].Name, userFolder.subfolders[randomFolder].name, missionTemplate.type);
        }
        else
        {
            int randomApp = Random.Range(0, apps.Count);
            missionText = missionText.Replace("{app}", apps[randomApp]);
            currentMission = new ActiveMission(file, users[randomUser].Name, "", missionTemplate.type);

        }
        textMails.text = missionText;
        onMissionCreated.Invoke(missionText);
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

            if (folderObject.subfolders[randomFolder].subfolders.Count == 0 && folderObject.subfolders[randomFolder].name != folder) folderObject.subfolders[randomFolder].files.Add(oSFile);
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
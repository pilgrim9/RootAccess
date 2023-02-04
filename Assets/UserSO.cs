using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CreateAssetMenu(fileName = "UserSO", menuName = "New User")]
public class UserSO : ScriptableObject
{
    public string Name;
    public List<OSFolder> folders;
    public List<OSFile> files;
}

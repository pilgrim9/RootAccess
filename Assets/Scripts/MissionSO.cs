using UnityEngine;
[CreateAssetMenu(fileName = "MissionSO", menuName = "New Mission")]
public class MissionSO : ScriptableObject
{
    [TextAreaAttribute(3, 10)]
    public string text;

    public MissionType type;
}
public enum MissionType
{
    Move,
    Install
}
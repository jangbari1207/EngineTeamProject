using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Step")]
public class QuestStepData : ScriptableObject
{
    public string questName;
    [TextArea]public string npcDialogue;
    public string[] turnOnIds;          
    public string[] turnOffIds;
}

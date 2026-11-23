using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Step")]
public class QuestStepData : ScriptableObject
{
    public string questCode;           
    [TextArea] public string npcDialogue;
    public ItemSO requiredItem;         
    public string[] turnOnIds;
    public string[] turnOffIds;
}

using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Step")]
public class QuestStepData : ScriptableObject
{
    public int stepNumber;
    public string questCode;           
    [TextArea] public string content;
    [TextArea] public string goal;
    public string transformationNote;
    [TextArea] public string npcDialogue;
    public ItemSO requiredItem;         
    public string[] turnOnIds;
    public string[] turnOffIds;
}

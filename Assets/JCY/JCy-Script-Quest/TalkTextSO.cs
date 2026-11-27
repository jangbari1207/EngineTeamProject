using UnityEngine;

[CreateAssetMenu(fileName = "TalkText", menuName = "Talk/Talk Text")]
public class TalkTextSO : ScriptableObject
{
    public string speakerName;
    [TextArea(2, 5)] public string dialogueText;
}

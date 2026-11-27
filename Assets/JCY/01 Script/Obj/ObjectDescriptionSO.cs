using UnityEngine;

[CreateAssetMenu(fileName = "NewObjectDescription", menuName = "Object/Description")]
public class ObjectDescriptionSO : ScriptableObject
{
    public string objectName;
    [TextArea(2, 6)] public string description;
}

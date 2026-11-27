using UnityEngine;

public class ObjectInteraction_JCY : MonoBehaviour, IObject_JCY, IObjectDescription_JCY
{
    [SerializeField] private ObjectDescriptionSO objectInfo;
    [SerializeField] private QuestStepData questStep;
    [SerializeField] private TalkManager talkManager;

    public void Interaction(PlayerInterecttion_JCY player, FadePanel_JCY fadePanelJcy = null)
    {
        if (questStep != null && QuestManager.instance != null)
            QuestManager.instance.ReportSignal(questStep);
    }

    public void ShowDescription()
    {
        if (objectInfo == null)
        {
            Debug.LogWarning($"Object description SO is not assigned on {name}.", this);
            return;
        }

        if (talkManager != null)
            talkManager.ShowObjectInfo(objectInfo.objectName, objectInfo.description);
    }
}

using UnityEngine;

/// <summary>Quest interaction used by PlayerInterecttion_JCY in Test_JCY.</summary>
public class QuestInteraction_JCY : MonoBehaviour, IObject_JCY
{
    [SerializeField] private QuestStepData questStep;
    [SerializeField] private TalkManager talkManager;

    public void Interaction(PlayerInterecttion_JCY player, FadePanel_JCY fadePanelJcy = null)
    {
        if (QuestManager.instance == null)
        {
            Debug.LogWarning("QuestManager is missing from this scene.", this);
            return;
        }

        bool completed = QuestManager.instance.ReportSignal(questStep);
        if (talkManager != null)
        {
            string message = completed
                ? "사각형과 상호작용해 첫 번째 퀘스트를 완료했습니다. 다음 목표를 확인하세요."
                : "이 사각형은 현재 진행 중인 퀘스트 단계와 연결되어 있지 않습니다.";
            talkManager.TalkStart("퀘스트 테스트", message);
        }
    }
}

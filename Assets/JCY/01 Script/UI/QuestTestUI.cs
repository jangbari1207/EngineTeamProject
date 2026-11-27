using TMPro;
using UnityEngine;

/// <summary>Displays the active quest content in the existing QuestCanvas.</summary>
public class QuestTestUI : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text contentText;

    private QuestManager questManager;

    private void Start()
    {
        questManager = QuestManager.instance;
        if (questManager == null)
        {
            Debug.LogError("QuestManager is missing from this scene.", this);
            return;
        }

        if (titleText == null || contentText == null)
            return;

        questManager.OnQuestChanged += Refresh;
        Refresh();
    }

    private void OnDestroy()
    {
        if (questManager != null)
            questManager.OnQuestChanged -= Refresh;
    }

    private void Refresh()
    {
        if (questManager == null || titleText == null || contentText == null)
            return;

        titleText.text = "현재 목표";

        if (questManager.steps == null || questManager.currentStep < 0 ||
            questManager.currentStep >= questManager.steps.Count)
        {
            contentText.text = string.Empty;
            return;
        }

        QuestStepData step = questManager.steps[questManager.currentStep];
        contentText.text = step != null ? step.content : string.Empty;
    }
}

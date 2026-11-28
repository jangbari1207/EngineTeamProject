using System.Collections;
using UnityEngine;

public class QuestTalkZone_JCY : MonoBehaviour
{
    [SerializeField] private QuestStepData requiredQuest;
    [SerializeField] private QuestStepData completeQuest;
    [SerializeField] private TalkTextSO[] dialogueLines;
    [SerializeField, Min(0f)] private float delayBetweenLines = 3f;
    [SerializeField] private bool triggerOnExit;

    private bool hasStarted;
    private bool isWaitingForQuest;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (triggerOnExit || hasStarted || !other.CompareTag("Player"))
            return;

        QuestManager manager = QuestManager.instance;
        if (IsRequiredQuestActive(manager))
            StartDialogue(manager);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!triggerOnExit || hasStarted || isWaitingForQuest || !other.CompareTag("Player"))
            return;

        QuestManager manager = QuestManager.instance;
        if (IsRequiredQuestActive(manager))
        {
            StartDialogue(manager);
            return;
        }

        if (manager != null && manager.steps != null && requiredQuest != null)
            StartCoroutine(WaitForRequiredQuest(manager));
    }

    private bool IsRequiredQuestActive(QuestManager manager)
    {
        return manager != null && requiredQuest != null && completeQuest != null &&
               manager.steps != null && manager.currentStep >= 0 &&
               manager.currentStep < manager.steps.Count &&
               manager.steps[manager.currentStep] == requiredQuest;
    }

    private void StartDialogue(QuestManager manager)
    {
        hasStarted = true;
        StartCoroutine(PlayDialogue(manager));
    }

    private IEnumerator WaitForRequiredQuest(QuestManager manager)
    {
        int requiredStepIndex = manager.steps.IndexOf(requiredQuest);
        if (requiredStepIndex < 0)
            yield break;

        isWaitingForQuest = true;
        while (!hasStarted && manager != null && manager.currentStep < requiredStepIndex)
            yield return null;

        isWaitingForQuest = false;
        if (!hasStarted && IsRequiredQuestActive(manager))
            StartDialogue(manager);
    }

    private IEnumerator PlayDialogue(QuestManager manager)
    {
        TalkManager talkManager = TalkManager.Instance;
        if (talkManager != null && dialogueLines != null)
        {
            for (int i = 0; i < dialogueLines.Length; i++)
            {
                TalkTextSO line = dialogueLines[i];
                if (line == null)
                    continue;

                bool isLastLine = true;
                for (int nextIndex = i + 1; nextIndex < dialogueLines.Length; nextIndex++)
                {
                    if (dialogueLines[nextIndex] != null)
                    {
                        isLastLine = false;
                        break;
                    }
                }

                yield return talkManager.ShowTalkRoutine(line, isLastLine);

                if (!isLastLine)
                    yield return talkManager.WaitForNextLine(delayBetweenLines);
            }

            yield return talkManager.WaitForNextLine(1.5f);
            talkManager.CloseTalk();
        }

        manager.ReportSignal(completeQuest);
    }
}

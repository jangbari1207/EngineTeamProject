using System.Collections;
using UnityEngine;

public class QuestMaidDarknessSequence_JCY : MonoBehaviour
{
    [SerializeField] private Eye_JCY playerEye;
    [SerializeField] private QuestStepData questStep4;
    [SerializeField] private QuestStepData questStep5;
    [SerializeField] private QuestStepData questStep6;
    [SerializeField] private TalkTextSO toolsPickedUpLine;
    [SerializeField] private TalkTextSO darknessLine;
    [SerializeField] private TalkTextSO restoredLightLine;
    [SerializeField, Min(0f)] private float darknessRadius = 7f;
    [SerializeField, Min(0f)] private float darknessDuration = 5f;
    [SerializeField, Min(0f)] private float delayBeforeLeaveQuest = 5f;
    [SerializeField, Min(0f)] private float finalLineHoldDuration = 1.5f;

    private bool hasStarted;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (hasStarted || !other.CompareTag("Player"))
            return;

        QuestManager manager = QuestManager.instance;
        if (manager == null || questStep4 == null || manager.steps == null ||
            manager.currentStep < 0 || manager.currentStep >= manager.steps.Count ||
            manager.steps[manager.currentStep] != questStep4)
            return;

        hasStarted = true;
        StartCoroutine(PlaySequence(manager));
    }

    private IEnumerator PlaySequence(QuestManager manager)
    {
        TalkManager talkManager = TalkManager.Instance;
        yield return PlayLine(talkManager, toolsPickedUpLine);

        if (!manager.ReportSignal(questStep4))
            yield break;

        float darknessStartedAt = Time.time;
        if (playerEye != null)
            playerEye.OverrideStoryLightRadius(darknessRadius);

        yield return PlayLine(talkManager, darknessLine);

        float remainingDarkness = darknessDuration - (Time.time - darknessStartedAt);
        if (remainingDarkness > 0f)
            yield return new WaitForSeconds(remainingDarkness);

        if (playerEye != null)
            playerEye.RestoreStoryLightRadius();

        if (!manager.ReportSignal(questStep5))
            yield break;

        yield return PlayLine(talkManager, restoredLightLine);
        yield return new WaitForSeconds(delayBeforeLeaveQuest);
        manager.ReportSignal(questStep6);
    }

    private IEnumerator PlayLine(TalkManager talkManager, TalkTextSO line)
    {
        if (talkManager == null || line == null)
            yield break;

        yield return talkManager.ShowTalkRoutine(line, true);
        yield return talkManager.WaitForNextLine(finalLineHoldDuration);
        talkManager.CloseTalk();
    }

    private void OnDisable()
    {
        if (playerEye != null)
            playerEye.RestoreStoryLightRadius();
    }
}

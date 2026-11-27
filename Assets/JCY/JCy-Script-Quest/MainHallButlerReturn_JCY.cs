using System.Collections;
using UnityEngine;

public class MainHallButlerReturn_JCY : MonoBehaviour
{
    [SerializeField] private QuestStepData mainHallQuest;
    [SerializeField] private QuestStepData findButlerQuest;
    [SerializeField] private GameObject butlerPrefab;
    [SerializeField] private Transform butlerSpawnPoint;
    [SerializeField] private TalkTextSO butlerLastWords;
    [SerializeField, Min(0f)] private float deathDelayAfterLine = 1.5f;

    private bool hasStarted;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (hasStarted || !other.CompareTag("Player"))
            return;

        QuestManager manager = QuestManager.instance;
        if (manager == null || mainHallQuest == null || manager.steps == null ||
            manager.currentStep < 0 || manager.currentStep >= manager.steps.Count ||
            manager.steps[manager.currentStep] != mainHallQuest)
            return;

        hasStarted = true;
        StartCoroutine(PlaySequence(manager));
    }

    private IEnumerator PlaySequence(QuestManager manager)
    {
        if (!manager.ReportSignal(mainHallQuest))
            yield break;

        GameObject butler = null;
        if (butlerPrefab != null && butlerSpawnPoint != null)
        {
            butler = Instantiate(butlerPrefab, butlerSpawnPoint.position, butlerSpawnPoint.rotation);
            Rigidbody2D body = butler.GetComponent<Rigidbody2D>();
            if (body != null)
                body.simulated = false;
        }

        TalkManager talkManager = TalkManager.Instance;
        if (talkManager != null && butlerLastWords != null)
        {
            yield return talkManager.ShowTalkRoutine(butlerLastWords, true);
            yield return talkManager.WaitForNextLine(deathDelayAfterLine);
            talkManager.CloseTalk();
        }

        if (butler != null)
            Destroy(butler);

        if (manager != null && findButlerQuest != null && manager.steps != null &&
            manager.currentStep < manager.steps.Count && manager.steps[manager.currentStep] == findButlerQuest)
            manager.ReportSignal(findButlerQuest);

    }
}

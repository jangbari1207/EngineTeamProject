using System.Collections;
using UnityEngine;

public class FirstFloorButlerIntro_JCY : MonoBehaviour
{
    [SerializeField] private GameObject butlerPrefab;
    [SerializeField] private Door_JCY receptionExitDoor;
    [SerializeField] private QuestStepData firstQuest;
    [SerializeField] private TalkTextSO[] dialogueLines;
    [SerializeField, Min(0f)] private float arrivalDelay = 10f;
    [SerializeField, Min(0f)] private float greetingDelay = 2f;
    [SerializeField, Min(0f)] private float nextLineDelay = 3f;
    [SerializeField, Min(0.1f)] private float butlerWalkSpeed = 2.5f;

    private void Start()
    {
        if (receptionExitDoor == null || receptionExitDoor._doorTfm == null ||
            butlerPrefab == null || firstQuest == null)
        {
            Debug.LogError("응접실 퀘스트 시작 오브젝트 참조가 설정되지 않았습니다.", this);
            return;
        }

        receptionExitDoor.SetLocked(true);
        StartCoroutine(WelcomeButler());
    }

    private IEnumerator WelcomeButler()
    {
        yield return new WaitForSeconds(arrivalDelay);

        Transform doorTransform = receptionExitDoor.transform;
        Vector3 butlerDestination = receptionExitDoor._doorTfm.position;
        GameObject butler = Instantiate(butlerPrefab, doorTransform.position, Quaternion.identity);
        Rigidbody2D butlerBody = butler.GetComponent<Rigidbody2D>();
        if (butlerBody != null)
            butlerBody.simulated = false;

        foreach (Collider2D butlerCollider in butler.GetComponentsInChildren<Collider2D>())
            butlerCollider.enabled = false;

        Transform butlerTransform = butler.transform;
        while (Vector2.Distance(butlerTransform.position, butlerDestination) > 0.05f)
        {
            butlerTransform.position = Vector3.MoveTowards(
                butlerTransform.position,
                butlerDestination,
                butlerWalkSpeed * Time.deltaTime);
            yield return null;
        }

        butlerTransform.position = butlerDestination;
        yield return new WaitForSeconds(greetingDelay);

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
                    yield return talkManager.WaitForNextLine(nextLineDelay);
            }

            yield return talkManager.WaitForNextLine(1.5f);
            talkManager.CloseTalk();
        }

        receptionExitDoor.SetLocked(false);

        if (QuestManager.instance != null)
            QuestManager.instance.ReportSignal(firstQuest);
    }
}

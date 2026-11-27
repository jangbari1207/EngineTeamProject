using System.Collections;
using TMPro;
using UnityEngine;

public class FirstFloorButlerIntro : MonoBehaviour
{
    [SerializeField] private GameObject butlerPrefab;
    [SerializeField] private Door_JCY receptionExitDoor;
    [SerializeField] private QuestStepData firstQuest;
    [SerializeField] private TMP_FontAsset dialogueFont;
    [SerializeField, Min(0f)] private float arrivalDelay = 10f;
    [SerializeField, Min(0f)] private float greetingDelay = 2f;
    [SerializeField, Min(0f)] private float nextLineDelay = 3f;
    [SerializeField, Min(0.01f)] private float secondsPerCharacter = 0.06f;
    [SerializeField, Min(0.1f)] private float butlerWalkSpeed = 2.5f;

    private const string GreetingLine = "필리아 아가씨 안녕하신가요";
    private const string MeetingLine = "아버지를 만나러 가실 시간입니다";

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

        // 대사가 끝날 때까지 출구 잠금은 유지한다.
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

        TextMeshPro dialogueText = CreateDialogueText(butlerTransform);
        yield return new WaitForSeconds(greetingDelay);
        yield return TypeLine(dialogueText, GreetingLine);
        yield return new WaitForSeconds(nextLineDelay);
        yield return TypeLine(dialogueText, MeetingLine);

        receptionExitDoor.SetLocked(false);

        if (QuestManager.instance != null)
            QuestManager.instance.ReportSignal(firstQuest);

        yield return new WaitForSeconds(2f);
        if (dialogueText != null)
            Destroy(dialogueText.gameObject);
    }

    private TextMeshPro CreateDialogueText(Transform butlerTransform)
    {
        GameObject textObject = new GameObject("Butler Dialogue Text", typeof(TextMeshPro));
        textObject.transform.SetParent(butlerTransform, false);
        textObject.transform.localPosition = new Vector3(0f, 0.7f, -0.2f);
        textObject.transform.localScale = Vector3.one * 0.1f;

        TextMeshPro dialogueText = textObject.GetComponent<TextMeshPro>();
        dialogueText.font = dialogueFont;
        dialogueText.fontSize = 25f;
        dialogueText.alignment = TextAlignmentOptions.Center;
        dialogueText.color = Color.white;
        dialogueText.outlineColor = Color.black;
        dialogueText.outlineWidth = 0.15f;
        dialogueText.enableWordWrapping = false;
        dialogueText.text = string.Empty;

        MeshRenderer textRenderer = textObject.GetComponent<MeshRenderer>();
        if (textRenderer != null)
            textRenderer.sortingOrder = 50;

        return dialogueText;
    }

    private IEnumerator TypeLine(TextMeshPro dialogueText, string line)
    {
        yield return TextTypewriter.Instance.TypeText(dialogueText, line, secondsPerCharacter);
    }
}

using UnityEngine;

public class QuestZone : MonoBehaviour
{
    [SerializeField] private string questCode;  

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            QuestManager.instance.TryComplete(questCode);
    }
}

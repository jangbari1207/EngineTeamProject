using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestGardenArrival_JCY : MonoBehaviour
{
    [SerializeField] private QuestStepData gardenQuest;

    private bool hasCompleted;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (hasCompleted || SceneManager.GetActiveScene().name != "First_Floor" ||
            !other.CompareTag("Player"))
            return;

        QuestManager manager = QuestManager.instance;
        if (manager == null || gardenQuest == null || manager.steps == null ||
            manager.currentStep < 0 || manager.currentStep >= manager.steps.Count ||
            manager.steps[manager.currentStep] != gardenQuest)
            return;

        hasCompleted = manager.ReportSignal(gardenQuest);
    }
}

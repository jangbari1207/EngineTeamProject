using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public List<QuestStepData> steps;
    public int currentStep = 0;

    public event Action OnQuestChanged;

    private Dictionary<string, GameObject> targets = new Dictionary<string, GameObject>();

    private void Awake()
    {
        instance = this;
        QuestTarget[] found = FindObjectsByType<QuestTarget>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (QuestTarget t in found)
        {
            targets[t.id] = t.gameObject;
        }
    }
    private void Start()
    {
        StartStep();
    }
    private void Update()
    {
        if (currentStep >= steps.Count) return;
        if (InventoryManager.instance == null) return;

        ItemSO need = steps[currentStep].requiredItem;
        if (need != null && InventoryManager.instance.HasItem(need))
            CompleteStep();
    }
    private void StartStep()
    {
        if (currentStep >= steps.Count) return;

        QuestStepData step = steps[currentStep];
        Debug.Log(step.npcDialogue);
        SetActiveByIds(step.turnOnIds, true);
        SetActiveByIds(step.turnOffIds, false);

        OnQuestChanged?.Invoke();
    }
    private void SetActiveByIds(string[] ids, bool active)
    {
        foreach(string id in ids)
        {
            if (targets.ContainsKey(id))
                targets[id].SetActive(active);
            else
                Debug.Log("Id를 찾을 수 없음: " + id);
        }
    }
    public void TryComplete(string questCode)
    {
        if (string.IsNullOrWhiteSpace(questCode)) return;
        if (steps == null || currentStep >= steps.Count) return;

        // Compatibility path for existing QuestZone / LockedFloor components.
        if (steps[currentStep].questCode == questCode)
            CompleteStep();
    }

    public bool ReportSignal(QuestStepData stepData)
    {
        if (stepData == null) return false;
        if (steps == null || currentStep >= steps.Count) return false;

        // The SO reference assigned to the source must be the active step.
        if (steps[currentStep] != stepData) return false;

        CompleteStep();
        return true;
    }
    public void CompleteStep()
    {
        Debug.Log($"Quest{currentStep} 완료!");
        currentStep++;
        StartStep();
    }
    public string GetDialogue()
    {
        if (currentStep >= steps.Count) return "더 이상 할 일이 없다.";
        return steps[currentStep].npcDialogue;
    }
}

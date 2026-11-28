using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Sends the assigned quest step to QuestManager when an event in the scene occurs.
/// Use Manual for animation, dialogue, or gameplay code to call EmitSignal().
/// </summary>
public class QuestSignalSource : MonoBehaviour
{
    public enum ActivationMode
    {
        Manual,
        TriggerEnter,
        Interact
    }

    [SerializeField] private QuestStepData signal;
    [SerializeField] private ActivationMode activationMode = ActivationMode.Manual;
    [SerializeField] private string actorTag = "Player";
    [SerializeField] private Key interactKey = Key.E;

    private bool actorInRange;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!MatchesActor(other)) return;

        if (activationMode == ActivationMode.TriggerEnter)
            EmitSignal();
        else if (activationMode == ActivationMode.Interact)
            actorInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (activationMode == ActivationMode.Interact && MatchesActor(other))
            actorInRange = false;
    }

    private void Update()
    {
        if (activationMode != ActivationMode.Interact || !actorInRange || Time.timeScale == 0)
            return;

        if (Keyboard.current != null && Keyboard.current[interactKey].wasPressedThisFrame)
            EmitSignal();
    }

    private bool MatchesActor(Collider2D other)
    {
        return other.CompareTag(actorTag);
    }

    /// <summary>Can be called by NPC, dialogue, animation, or gameplay scripts.</summary>
    public void EmitSignal()
    {
        if (signal == null)
        {
            Debug.LogWarning($"Quest step SO is not assigned on {name}.", this);
            return;
        }

        if (QuestManager.instance == null)
        {
            Debug.LogWarning("QuestManager is not present in this scene.", this);
            return;
        }

        QuestManager.instance.ReportSignal(signal);
    }
}

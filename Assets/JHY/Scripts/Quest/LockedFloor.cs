using UnityEngine;
using UnityEngine.InputSystem;

public class LockedFloor : MonoBehaviour
{
    [SerializeField] private ItemSO requiredKey;
    [SerializeField] private string questCode;
    private bool isPlayerInRange = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            isPlayerInRange = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            isPlayerInRange = false;
    }
    private void Update()
    {
        if (Time.timeScale == 0) return;

        if (isPlayerInRange && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (InventoryManager.instance.HasItem(requiredKey))
            {
                QuestManager.instance.TryComplete(questCode);
                InventoryManager.instance.UseItem(requiredKey);
                gameObject.SetActive(false);  
            }
            else
            {
                Debug.Log("열쇠가 필요하다.");   // 나중에 대사창으로 바꾸면 됨
            }
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class DoorFor2F : MonoBehaviour
{
    [SerializeField] private ItemSO requiredKey;
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
                //2층 이동 로직
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("열쇠가 필요하다.");   
            }
        }
    }
}

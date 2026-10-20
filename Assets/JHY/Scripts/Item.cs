using UnityEngine;
using UnityEngine.InputSystem;

public class Item : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private int quantity;
    [SerializeField] private Sprite sprite;
    private InventoryManager inventoryManager;
    private bool isPlayerInRange = false;

    private void Start()
    {
        inventoryManager = Object.FindAnyObjectByType<InventoryManager>();
        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager를 찾을 수 없습니다!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            if (inventoryManager != null)
            {
                inventoryManager.AddItem(itemName, quantity, sprite);
            }
            Destroy(gameObject);
        }
    }
}

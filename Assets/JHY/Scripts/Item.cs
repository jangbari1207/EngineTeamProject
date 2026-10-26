using UnityEngine;
using UnityEngine.InputSystem;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemSO itemData;
    [SerializeField] private int quantity = 1;
    private bool isPlayerInRange = false;
    public void Initialize(ItemSO data, int amount)
    {
        itemData = data;
        quantity = amount;
    }
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

        if (isPlayerInRange && Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            quantity = InventoryManager.instance.AddItem(itemData, quantity);
            if (quantity <= 0)
                Destroy(gameObject);
        }
    }
}

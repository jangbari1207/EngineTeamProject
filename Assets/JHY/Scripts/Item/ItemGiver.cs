using UnityEngine;
using UnityEngine.InputSystem;

public class ItemGiver : MonoBehaviour
{
    [SerializeField] private ItemSO itemData;      
    [SerializeField] private int quantity = 1;
    [SerializeField] private Sprite openedSprite;  
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
        if (Time.timeScale == 0 || quantity <= 0) return;  

        if (isPlayerInRange && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            quantity = InventoryManager.instance.AddItem(itemData, quantity);

            if (quantity <= 0 && openedSprite != null)
                GetComponent<SpriteRenderer>().sprite = openedSprite;
        }
    }
}

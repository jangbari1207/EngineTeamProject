using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;
    public ItemSlot[] itemSlot;
     
    private void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            menuActivated = !menuActivated;

            if (menuActivated)
            {
                Time.timeScale = 0;
                if (InventoryMenu != null) InventoryMenu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                if (InventoryMenu != null) InventoryMenu.SetActive(false);
            }
        }
    }

    public void AddItem(string itemName, int quantity, Sprite itemSprite)
    {
        for(int i = 0;i<itemSlot.Length;i++)
        {
            if (itemSlot[i].isFull == false)
            {
                itemSlot[i].AddItem(itemName, quantity, itemSprite);
                return;
            }
        }
    }
}

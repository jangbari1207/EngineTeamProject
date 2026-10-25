using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public GameObject InventoryMenu;
    public ItemSlot[] itemSlot;
    private bool menuActivated;
    private int currentFilter = -1;
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            menuActivated = !menuActivated;
            Time.timeScale = menuActivated ? 0 : 1;
            InventoryMenu.SetActive(menuActivated);
        }
    }
    public int AddItem(ItemSO itemData, int quantity)
    {
        for (int i = 0; i < itemSlot.Length && quantity > 0; i++)
        {
            if (!itemSlot[i].isFull && (itemSlot[i].itemData == itemData || itemSlot[i].quantity == 0))
            {
                quantity = itemSlot[i].AddItem(itemData, quantity);
            }
        }
        RefreshFilter();
        return quantity;
    }
    public void FilterSlots(int categoryIndex)  
    {
        currentFilter = categoryIndex;  
        DeselectAllSlots();            
        itemSlot[0].ClearDescription();  
        RefreshFilter();
    }

    public void RefreshFilter()
    {
        for (int i = 0; i < itemSlot.Length; i++)   
        {
            if (currentFilter == -1)
            {
                itemSlot[i].gameObject.SetActive(true);
            }
            else if (itemSlot[i].itemData != null && (int)itemSlot[i].itemData.category == currentFilter)
            {
                itemSlot[i].gameObject.SetActive(true);
            }
            else
            {
                itemSlot[i].gameObject.SetActive(false);
            }
        }
    }
    public void DeselectAllSlots()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }
    }
}

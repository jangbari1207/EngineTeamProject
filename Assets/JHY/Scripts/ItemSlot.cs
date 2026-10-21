using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour,IPointerClickHandler
{
    [Header("Item Data")]
    public ItemSO itemData;
    public int quantity;
    public bool isFull;

    [Header("Item Slot")]
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;

    [Header("Item Description")]
    public Image itemDescriptionImage;
    public TMP_Text ItemDescriptionNameText;
    public TMP_Text ItemDescriptionText;

    public GameObject selectedShader;
    public bool thisItemSelected;
    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }
    public void AddItem(ItemSO itemData, int quantity)
    {
        this.itemData = itemData;
        this.quantity = quantity;
        isFull = true;

        quantityText.text = quantity.ToString();
        quantityText.enabled = true;
        itemImage.sprite = itemData.sprite;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }
    private void OnRightClick()
    {

    }
    public void OnLeftClick()
    {
        inventoryManager.DeselectAllSlots();
        selectedShader.SetActive(true);
        thisItemSelected = true;

        if (itemData == null) return;   // 빈 슬롯 클릭 방어

        ItemDescriptionNameText.text = itemData.itemName;
        ItemDescriptionText.text = itemData.itemDescription;
        itemDescriptionImage.sprite = itemData.sprite;
    }
}

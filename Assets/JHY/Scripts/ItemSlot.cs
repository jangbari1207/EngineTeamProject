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
    public Sprite empty;
    [Header("Item Slot")]
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;
    [SerializeField] private int maxNumberOfItems;
    [Header("Item Description")]
    public Image itemDescriptionImage;
    public TMP_Text ItemDescriptionNameText;
    public TMP_Text ItemDescriptionText;
    public GameObject selectedShader;
    public bool thisItemSelected;
    public int AddItem(ItemSO itemData, int quantity)
    {
        if (isFull)
            return quantity;

        this.itemData = itemData;
        itemImage.sprite = itemData.sprite;
        this.quantity += quantity;
        quantityText.enabled = true;

        if (this.quantity >= maxNumberOfItems)
        {
            int extraItems = this.quantity - maxNumberOfItems;
            this.quantity = maxNumberOfItems;
            quantityText.text = this.quantity.ToString();
            isFull = true;
            return extraItems;
        }

        quantityText.text = this.quantity.ToString();
        return 0;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            OnLeftClick();
        else if (eventData.button == PointerEventData.InputButton.Right)
            OnRightClick();
    }
    private void OnRightClick()
    {
        if (itemData == null) return;

        GameObject itemToDrop = new GameObject(itemData.itemName);
        itemToDrop.AddComponent<Item>().Initialize(itemData, 1);

        SpriteRenderer sr = itemToDrop.AddComponent<SpriteRenderer>();
        sr.sprite = itemData.sprite;
        sr.sortingOrder = 5;
        sr.sortingLayerName = "Ground";

        itemToDrop.AddComponent<BoxCollider2D>().isTrigger = true;
        itemToDrop.transform.position = PlayerHealth.instance.transform.position;
        itemToDrop.transform.localScale = new Vector3(0.1282434f, 0.1059094f, 0.5054f);

        RemoveOne();
    }
    public void OnLeftClick()
    {
        if (thisItemSelected && itemData != null)
        {
            if (itemData.UseItem())
                RemoveOne();
            return;
        }

        InventoryManager.instance.DeselectAllSlots();
        selectedShader.SetActive(true);
        thisItemSelected = true;

        if (itemData == null)
        {
            ClearDescription();
            return;
        }

        ItemDescriptionNameText.text = itemData.itemName;
        ItemDescriptionText.text = itemData.itemDescription;
        itemDescriptionImage.sprite = itemData.sprite;
    }
    private void RemoveOne()
    {
        quantity--;
        isFull = false;
        if (quantity <= 0)
            EmptySlot();
        else
            quantityText.text = quantity.ToString();
    }
    private void EmptySlot()
    {
        itemData = null;
        quantity = 0;
        quantityText.enabled = false;
        itemImage.sprite = empty;
        ClearDescription();
        InventoryManager.instance.RefreshFilter();
    }
    public void ClearDescription()
    {
        ItemDescriptionNameText.text = "";
        ItemDescriptionText.text = "";
        itemDescriptionImage.sprite = empty;
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField]
    private Image itemIcon;

    [SerializeField]
    private TextMeshProUGUI quantityText;

    [SerializeField]
    private Button slotButton;

    [SerializeField]
    private Image selectionHighlight;

    private int slotIndex;
    private InventoryItem currentItem;
    private InventoryUI inventoryUI;

    private void Start()
    {
        if (slotButton != null)
            slotButton.onClick.AddListener(OnSlotClick);
        if (selectionHighlight != null)
            selectionHighlight.gameObject.SetActive(false);
    }

    public void Initialize(int index, InventoryUI parent)
    {
        slotIndex = index;
        inventoryUI = parent;
    }

    public void UpdateSlot(InventoryItem item)
    {
        currentItem = item;

        if (item == null)
        {
            if (itemIcon != null)
            {
                itemIcon.sprite = null;
                itemIcon.enabled = false;
            }
            if (quantityText != null)
                quantityText.text = "";
        }
        else
        {
            if (itemIcon != null)
            {
                itemIcon.sprite = item.Icon;
                itemIcon.enabled = true;
            }
            if (quantityText != null)
                quantityText.text = item.Quantity > 1 ? item.Quantity.ToString() : "";
        }
    }

    private void OnSlotClick()
    {
        if (inventoryUI != null)
        {
            inventoryUI.SelectSlot(slotIndex, this);
        }
    }

    public void SetSelected(bool selected)
    {
        if (selectionHighlight != null)
            selectionHighlight.gameObject.SetActive(selected);
    }

    public int GetSlotIndex() => slotIndex;
    public InventoryItem GetItem() => currentItem;
}

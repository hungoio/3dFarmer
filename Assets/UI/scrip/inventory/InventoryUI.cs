using UnityEngine;
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
#endif
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private Transform slotsContainer;
    [SerializeField]
    private GameObject slotPrefab;
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private TextMeshProUGUI itemNameText;
    [SerializeField]
    private TextMeshProUGUI itemDescriptionText;
    [SerializeField]
    private TextMeshProUGUI itemQuantityText;
    [SerializeField]
    private Image itemIconDisplay;
    [SerializeField]
    private Button useButton;
    [SerializeField]
    private Button sellButton;

    [SerializeField]
    // The root GameObject that represents the visible UI to toggle.
    // Do NOT set this to the same GameObject that holds this script, otherwise
    // disabling it will stop Update() and keyboard input won't reopen the UI.
    private GameObject uiRoot;

    private List<InventorySlotUI> slotUIs = new List<InventorySlotUI>();
    private InventorySlotUI selectedSlot;

    private void Start()
    {
        if (inventory == null)
        {
            Debug.LogWarning("Inventory reference is null on InventoryUI. Inventory functionality will be disabled.");
            // continue without disabling this component so keyboard input remains available
        }

        if (slotsContainer == null && slotPrefab == null)
        {
            Debug.LogWarning("Both slotsContainer and slotPrefab are null on InventoryUI. Nothing to initialize.");
            return;
        }

        InitializeSlots();
        
        if (inventory != null)
            inventory.OnInventoryChanged += RefreshInventoryDisplay;
        
        if (useButton != null)
            useButton.onClick.AddListener(OnUseItemClicked);
        if (sellButton != null)
            sellButton.onClick.AddListener(OnSellItemClicked);

        RefreshInventoryDisplay();
    }

    private void InitializeSlots()
    {
        if (inventory == null)
        {
            Debug.LogWarning("Inventory is null in InitializeSlots()");
            return;
        }

        int inventorySize = inventory.GetInventorySize();

        if (slotPrefab != null)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                GameObject slotGO = Instantiate(slotPrefab, slotsContainer);
                InventorySlotUI slotUI = slotGO.GetComponent<InventorySlotUI>();
                if (slotUI != null)
                {
                    slotUI.Initialize(i, this);
                    slotUIs.Add(slotUI);
                }
            }
        }
        else
        {
            foreach (Transform child in slotsContainer)
            {
                InventorySlotUI slotUI = child.GetComponent<InventorySlotUI>();
                if (slotUI != null)
                {
                    int index = child.GetSiblingIndex();
                    slotUI.Initialize(index, this);
                    slotUIs.Add(slotUI);
                }
            }
        }

        Debug.Log($"Initialized {slotUIs.Count} inventory slots");
    }

    private void RefreshInventoryDisplay()
    {
        if (inventory == null)
        {
            Debug.LogWarning("Cannot refresh inventory display: inventory is null");
            return;
        }

        for (int i = 0; i < slotUIs.Count; i++)
        {
            InventoryItem item = inventory.GetItemFromSlot(i);
            slotUIs[i].UpdateSlot(item);
        }
    }

    public void SelectSlot(int slotIndex, InventorySlotUI slotUI)
    {
        if (selectedSlot != null)
            selectedSlot.SetSelected(false);

        selectedSlot = slotUI;
        slotUI.SetSelected(true);

        InventoryItem item = inventory.GetItemFromSlot(slotIndex);
        UpdateItemInfo(item);
    }

    private void UpdateItemInfo(InventoryItem item)
    {
        if (item == null)
        {
            itemNameText.text = "Empty";
            itemDescriptionText.text = "";
            itemQuantityText.text = "";
            itemIconDisplay.sprite = null;
            itemIconDisplay.enabled = false;
            return;
        }

        itemNameText.text = item.Name;
        itemDescriptionText.text = item.Description;
        itemQuantityText.text = $"Quantity: {item.Quantity}/{item.MaxStackSize}";
        itemIconDisplay.sprite = item.Icon;
        itemIconDisplay.enabled = true;
    }

    private void OnUseItemClicked()
    {
        if (selectedSlot == null)
        {
            Debug.LogWarning("Không có item được chọn");
            return;
        }

        InventoryItem item = selectedSlot.GetItem();
        if (item == null) return;

        Debug.Log($"Using {item.Name}");

        if (item.ItemType == "Seed")
        {
            inventory.RemoveItem(item.Id, 1);
            Debug.Log($"Planted {item.Name}");
        }
    }

    private void OnSellItemClicked()
    {
        if (selectedSlot == null)
        {
            Debug.LogWarning("Không có item được chọn");
            return;
        }

        InventoryItem item = selectedSlot.GetItem();
        if (item == null) return;

        if (!item.IsTradeable)
        {
            Debug.LogWarning($"{item.Name} không thể bán");
            return;
        }

        Debug.Log($"Selling {item.Name} for {item.SellPrice} each");
    }

    public void MoveItemToSlot(int fromSlot, int toSlot)
    {
        inventory.MoveItem(fromSlot, toSlot);
    }

    public void SwapItemSlots(int slot1, int slot2)
    {
        inventory.SwapItems(slot1, slot2);
    }

    // Input handling moved to InventoryManager to avoid duplicate listeners.
    // Keep ToggleVisibility() public so InventoryManager or other controllers can open/close the UI.

    // Toggle the active state of this inventory UI. Can be called from other scripts or UI buttons.
    public void ToggleVisibility()
    {
        GameObject target = uiRoot;

        if (target == null)
        {
            // try to find a Canvas child to toggle instead of disabling this script's GameObject
            var canvas = GetComponentInChildren<Canvas>(true);
            if (canvas != null)
                target = canvas.gameObject;
        }

        if (target == null)
        {
            Debug.LogWarning("uiRoot is not assigned and no child Canvas found. Toggling this GameObject will disable input; assign uiRoot to avoid that.");
            target = gameObject;
        }

        bool isActive = target.activeSelf;
        target.SetActive(!isActive);
    }

    private void OnDestroy()
    {
        if (inventory != null)
            inventory.OnInventoryChanged -= RefreshInventoryDisplay;
    }
}

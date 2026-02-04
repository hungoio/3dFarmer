using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup inventoryPanel; // Drag InventoryPanel vào đây

    [SerializeField]
    private InventoryUI inventoryUI; // Reference to InventoryUI script

    [SerializeField]
    private Button closeButton;

    private bool isInventoryOpen = false;

    private void Start()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(CloseInventory);

        // Ẩn inventory lúc khởi động
        if (inventoryPanel != null)
            SetInventoryActive(false);
    }

    private void Update()
    {
        // Bật/tắt inventory khi nhấn phím I
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            ToggleInventory();
        }

        // Đóng inventory khi nhấn ESC
        if (Keyboard.current.escapeKey.wasPressedThisFrame && isInventoryOpen)
        {
            CloseInventory();
        }
    }

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        SetInventoryActive(isInventoryOpen);

        // Also toggle InventoryUI if reference is set
        if (inventoryUI != null)
            inventoryUI.ToggleVisibility();
    }

    public void OpenInventory()
    {
        SetInventoryActive(true);
    }

    public void CloseInventory()
    {
        SetInventoryActive(false);
    }

    private void SetInventoryActive(bool active)
    {
        isInventoryOpen = active;

        if (inventoryPanel != null)
        {
            inventoryPanel.gameObject.SetActive(active);
            inventoryPanel.blocksRaycasts = active;
            inventoryPanel.interactable = active;
        }

        Debug.Log(active ? "Inventory Mở" : "Inventory Đóng");
    }

    public bool IsInventoryOpen() => isInventoryOpen;
}

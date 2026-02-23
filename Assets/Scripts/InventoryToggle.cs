using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryPanel; // Kéo cái InventoryPanel vào đây

    void Start()
    {
        // Đảm bảo khi game bắt đầu thì Balo luôn đóng
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }
    }

    void Update()
    {
        // Bấm phím E để Bật hoặc Tắt
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        if (inventoryPanel != null)
        {
            // Đảo ngược trạng thái: Đang mở -> Tắt, Đang tắt -> Mở
            bool isActive = inventoryPanel.activeSelf;
            inventoryPanel.SetActive(!isActive);
        }
    }
}
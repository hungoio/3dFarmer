using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public Transform container;   // Cái lưới (Grid) để chứa các ô
    public GameObject slotPrefab; // Mẫu ô đồ (InventorySlot)

    // Danh sách TẤT CẢ vật phẩm có trong game (Bạn phải kéo EggData vào đây)
    public List<ItemData> allItems;

    void Start()
    {
        // 1. Xóa sạch các ô mẫu cũ (nếu có)
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        // 2. Tạo ô mới cho từng loại vật phẩm
        foreach (var item in allItems)
        {
            GameObject newSlot = Instantiate(slotPrefab, container);
            InventorySlot slotScript = newSlot.GetComponent<InventorySlot>();

            if (slotScript != null)
            {
                slotScript.Setup(item);
            }
        }
    }
}
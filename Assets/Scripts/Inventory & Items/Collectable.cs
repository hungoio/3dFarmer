using UnityEngine;

public class Collectable : MonoBehaviour
{
    [Header("Dữ liệu vật phẩm")]
    public ItemData data; // Kéo file EggData vào đây
    public int amount = 1; // Số lượng nhận được mỗi lần nhặt

    [Header("Hiệu ứng (Tùy chọn)")]
    public GameObject popupPrefab; // Kéo Prefab VFX_Harvest vào đây để có hiệu ứng +1

    // Hàm này sẽ được cái Rổ (HarvestDragTool) gọi khi nó quét trúng quả trứng
    public void Collect()
    {
        if (InventoryManager.Instance != null && data != null)
        {
            // 1. Thêm đồ vào kho (Balo)
            InventoryManager.Instance.AddItem(data.itemName, amount);

            // 2. Tạo hiệu ứng bay bay (+1) nịnh mắt
            if (popupPrefab != null)
            {
                // Sinh ra hiệu ứng cao hơn quả trứng 1 chút
                GameObject vfx = Instantiate(popupPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
                vfx.GetComponent<FloatingItem>().Setup(data.icon, amount);
            }

            // 3. Xóa quả trứng trên mặt đất đi
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("Thiếu InventoryManager hoặc chưa gắn ItemData cho quả trứng!");
        }
    }
}
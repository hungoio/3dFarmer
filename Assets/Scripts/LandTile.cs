using UnityEngine;

public class LandTile : MonoBehaviour
{
    // 👇 Sửa tên biến thành gridX, gridZ để GridManager không báo lỗi nữa
    public int gridX;
    public int gridZ;

    public CropInstance currentCrop;
    public GameObject popupPrefab; // Kéo Prefab VFX_Harvest vào đây

    // Biến để xử lý đổi màu khi chọn (Highlight)
    private Renderer tileRenderer;
    private Color originalColor;
    public Color selectionColor = Color.green; // Màu khi được chọn

    void Start()
    {
        tileRenderer = GetComponent<Renderer>();
        if (tileRenderer != null)
        {
            originalColor = tileRenderer.material.color;
        }
    }

    // ==========================================
    // 👇 CÁC HÀM BỊ THIẾU (NGUYÊN NHÂN LỖI ĐỎ) 👇
    // ==========================================

    // 1. Kiểm tra xem đất có trống không
    public bool IsEmpty()
    {
        return currentCrop == null;
    }

    // 2. Hàm khi chuột chọn vào ô đất (Đổi màu)
    public void Select()
    {
        if (tileRenderer != null)
            tileRenderer.material.color = selectionColor;
    }

    // 3. Hàm khi chuột bỏ chọn (Về màu cũ)
    public void Deselect()
    {
        if (tileRenderer != null)
            tileRenderer.material.color = originalColor;
    }

    // ==========================================
    // PHẦN LOGIC TRỒNG TRỌT & THU HOẠCH
    // ==========================================

    public void Harvest()
    {
        if (currentCrop == null || !currentCrop.IsReady()) return;

        CropData data = currentCrop.data;

        // Cộng đồ vào kho
        if (data.productItem != null)
        {
            InventoryManager.Instance.AddItem(data.productItem.itemName, data.yieldAmount);

            // 🔥 Hiệu ứng bay bay (+1)
            if (popupPrefab != null)
            {
                Vector3 spawnPos = transform.position + Vector3.up * 1f;
                GameObject vfx = Instantiate(popupPrefab, spawnPos, Quaternion.identity);
                // Setup hình ảnh và số lượng
                vfx.GetComponent<FloatingItem>().Setup(data.productItem.icon, data.yieldAmount);
            }
        }

        // Xóa cây
        Destroy(currentCrop.gameObject);
        currentCrop = null;

        // Lưu game
        FarmSaveManager.Instance.SaveTile(this);
    }

    // Hàm này cho PlantManager gọi để trồng cây
    public void Plant(CropInstance newCrop)
    {
        currentCrop = newCrop;
        FarmSaveManager.Instance.SaveTile(this);
    }
}
using UnityEngine;

public class AppleTree : MonoBehaviour
{
    [Header("Cấu hình")]
    public GameObject fruitGroup; // Kéo nhóm các quả táo trên cành vào đây
    public ItemData appleData;    // Kéo file AppleData vào đây
    public float growTime = 10f;  // 10 giây ra trái một lần

    private float timer;
    private bool hasFruit = false;

    void Start()
    {
        // Khi mới đặt cây, bắt đầu đếm giờ ngay
        timer = growTime;
        SetFruitState(false);
    }

    void Update()
    {
        if (!hasFruit)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SetFruitState(true);
            }
        }
    }

    // Hàm chuyển đổi giữa 2 trạng thái: Có quả và Không có quả
    void SetFruitState(bool state)
    {
        hasFruit = state;
        if (fruitGroup != null)
        {
            fruitGroup.SetActive(state); // Bật/Tắt model quả táo trên cành
        }
    }

    // Hàm thu hoạch (Sẽ được gọi từ ClickManager)
    public void Harvest()
    {
        if (hasFruit)
        {
            // 1. Thêm vào kho đồ
            InventoryManager.Instance.AddItem(appleData.itemName, 1);

            // 2. Chuyển về trạng thái không có quả
            SetFruitState(false);

            // 3. Reset đồng hồ điểm giờ
            timer = growTime;

            Debug.Log("Đã thu hoạch 1 quả táo!");
        }
    }
}
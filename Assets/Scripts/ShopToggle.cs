using UnityEngine;

public class ShopToggle : MonoBehaviour
{
    public GameObject shopPanel; // Kéo cái ShopPanel vào đây

    void Start()
    {
        // Tự động ẩn Shop ngay khi vào game
        if (shopPanel != null)
            shopPanel.SetActive(false);
    }

    void Update()
    {
        // Bấm phím B để Bật/Tắt
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleShop();
        }
    }

    public void ToggleShop()
    {
        bool isActive = shopPanel.activeSelf;
        shopPanel.SetActive(!isActive); // Đảo ngược trạng thái (Đang mở -> Tắt, Đang tắt -> Mở)
    }

    // Hàm này dùng cho nút "X" (Close Button)
    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }
}
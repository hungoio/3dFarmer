using UnityEngine;
using UnityEngine.UI;
using TMPro; // Nhớ dòng này để dùng Text xịn

public class InventorySlot : MonoBehaviour
{
    public ItemData data; // Dữ liệu của vật phẩm trong ô này

    [Header("UI")]
    public Image iconImage;
    public TMP_Text nameText;
    public TMP_Text countText;
    public TMP_Text priceText;
    public Button sellButton; // Nút bán

    void Update()
    {
        // Cập nhật số lượng liên tục
        if (data != null && InventoryManager.Instance != null)
        {
            int count = InventoryManager.Instance.GetItemCount(data.itemName);
            countText.text = "x" + count;

            // Nếu hết đồ (số lượng = 0) thì tắt nút bán đi cho đỡ bấm nhầm
            sellButton.interactable = (count > 0);
        }
    }

    // Hàm cài đặt thông tin (được gọi bởi InventoryUI)
    public void Setup(ItemData newData)
    {
        data = newData;
        if (data.icon != null) iconImage.sprite = data.icon;
        nameText.text = data.itemName;
        priceText.text = "+" + data.sellPrice + "$";
    }

    // Hàm BÁN ĐỒ (Gắn vào nút Sell)
    public void OnSellButton()
    {
        if (data == null || InventoryManager.Instance == null) return;

        // Kiểm tra xem có đồ để bán không
        if (InventoryManager.Instance.GetItemCount(data.itemName) > 0)
        {
            // 1. Trừ đồ trong kho
            // (Lưu ý: Bạn cần thêm hàm RemoveItem vào InventoryManager nếu chưa có, xem bên dưới)
            if (InventoryManager.Instance.itemStorage.ContainsKey(data.itemName))
            {
                InventoryManager.Instance.itemStorage[data.itemName]--;
            }

            // 2. Cộng tiền
            PlayerMoney.Instance.AddMoney(data.sellPrice);

            Debug.Log($"Đã bán 1 {data.itemName} giá {data.sellPrice}");
        }
    }
}
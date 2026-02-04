using UnityEngine;
using UnityEngine.UI;
using TMPro; // Nếu bạn dùng TextMeshPro, nếu dùng Text thường thì đổi thành Text

public class SeedShopUI : MonoBehaviour
{
    public CropData cropData;

    [Header("UI References")]
    public Text nameText;
    public Text priceText;
    public Text quantityText; // Hiển thị số lượng đang có
    public Image iconImage;

    void Start()
    {
        // Setup thông tin ban đầu
        if (cropData != null)
        {
            nameText.text = cropData.cropName;
            priceText.text = "$" + cropData.buyPrice;
            iconImage.sprite = cropData.prefab.GetComponentInChildren<SpriteRenderer>().sprite; // Lấy tạm hình từ prefab hoặc gán tay
        }
        UpdateQuantityUI();
    }

    void Update()
    {
        // Cập nhật số lượng liên tục (hơi tốn hiệu năng nhưng dễ làm nhất)
        UpdateQuantityUI();
    }

    void UpdateQuantityUI()
    {
        int count = InventoryManager.Instance.GetSeedCount(cropData.cropName);
        quantityText.text = "Kho: " + count;
    }

    // Gán hàm này vào sự kiện OnClick của nút chính (để chọn hạt trồng)
    public void OnClickSelect()
    {
        PlantManager.Instance.selectedCrop = cropData;
        Debug.Log("Đã chọn hạt: " + cropData.cropName);
    }

    // Gán hàm này vào nút nhỏ "BUY" bên cạnh (để mua)
    public void OnClickBuy()
    {
        int cost = cropData.buyPrice;

        if (PlayerMoney.Instance.money >= cost)
        {
            PlayerMoney.Instance.AddMoney(-cost); // Trừ tiền
            InventoryManager.Instance.AddSeed(cropData.cropName, 1); // Thêm hạt
        }
        else
        {
            Debug.Log("💸 Không đủ tiền!");
        }
    }
}
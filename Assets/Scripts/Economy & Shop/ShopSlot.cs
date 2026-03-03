using UnityEngine;
using UnityEngine.UI;
using TMPro; // 👈 QUAN TRỌNG: Thêm thư viện này để dùng Text xịn

public class ShopSlot : MonoBehaviour
{
    public CropData data;

    [Header("UI References - Kéo thả vào đây")]
    public Image iconImage;

    // 👇 ĐỔI HẾT 'Text' THÀNH 'TMP_Text' 👇
    public TMP_Text nameText;
    public TMP_Text priceText;
    public TMP_Text ownedText;

    void Start()
    {
        if (data != null)
        {
            // Lấy hình từ Prefab (nếu có SpriteRenderer)
            if (data.prefab.GetComponent<SpriteRenderer>() != null)
                iconImage.sprite = data.prefab.GetComponent<SpriteRenderer>().sprite;

            nameText.text = data.cropName;
            priceText.text = "$" + data.buyPrice;
        }
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (InventoryManager.Instance != null && data != null)
        {
            int count = InventoryManager.Instance.GetSeedCount(data.cropName);
            ownedText.text = "Có: " + count;
        }
    }

    public void OnBuyButton()
    {
        if (data == null) return;

        int cost = data.buyPrice;

        if (PlayerMoney.Instance.money >= cost)
        {
            PlayerMoney.Instance.AddMoney(-cost);
            InventoryManager.Instance.AddSeed(data.cropName, 1);
        }
        else
        {
            Debug.Log("Không đủ tiền!");
        }
    }

    public void OnSelectButton()
    {
        if (data == null) return;
        PlantManager.Instance.selectedCrop = data;
        Debug.Log("Đã chọn: " + data.cropName);
    }
}
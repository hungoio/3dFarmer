using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopUI : MonoBehaviour
{
    [SerializeField]
    private Shop shop;

    [SerializeField]
    private Inventory playerInventory;

    [SerializeField]
    private Transform shopItemsContainer; // Chứa danh sách item bán

    [SerializeField]
    private GameObject shopItemPrefab; // Prefab của item trong shop

    [SerializeField]
    private TextMeshProUGUI shopNameText;

    [SerializeField]
    private TextMeshProUGUI shopMoneyText;

    [SerializeField]
    private CanvasGroup shopPanel;

    private List<ShopItemUI> shopItemUIs = new List<ShopItemUI>();

    private void Start()
    {
        if (shopNameText != null)
            shopNameText.text = shop.GetShopName();

        shop.OnShopUpdated += RefreshShop;
        RefreshShop();
    }

    /// <summary>
    /// Làm mới danh sách shop
    /// </summary>
    private void RefreshShop()
    {
        // Xóa item cũ
        foreach (Transform child in shopItemsContainer)
        {
            Destroy(child.gameObject);
        }
        shopItemUIs.Clear();

        // Tạo item mới
        var items = shop.GetShopInventory();
        foreach (var shopItem in items)
        {
            if (shopItem.Quantity <= 0) continue;

            GameObject itemGO = Instantiate(shopItemPrefab, shopItemsContainer);
            ShopItemUI itemUI = itemGO.GetComponent<ShopItemUI>();
            if (itemUI != null)
            {
                itemUI.Initialize(shopItem, shop, playerInventory);
                shopItemUIs.Add(itemUI);
            }
        }

        // Cập nhật tiền shop
        if (shopMoneyText != null)
            shopMoneyText.text = $"Shop Money: {shop.GetShopMoney()}";
    }

    /// <summary>
    /// Mở/Đóng shop
    /// </summary>
    public void ToggleShop(bool active)
    {
        if (shopPanel != null)
            shopPanel.gameObject.SetActive(active);
    }

    private void OnDestroy()
    {
        if (shop != null)
            shop.OnShopUpdated -= RefreshShop;
    }
}

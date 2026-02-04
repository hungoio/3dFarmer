using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField]
    private Image itemIcon;

    [SerializeField]
    private TextMeshProUGUI itemNameText;

    [SerializeField]
    private TextMeshProUGUI priceText;

    [SerializeField]
    private Button buyButton;

    [SerializeField]
    private Button sellButton;

    private Shop.ShopItem shopItem;
    private Shop shop;
    private Inventory playerInventory;

    public void Initialize(Shop.ShopItem item, Shop shopRef, Inventory playerInv)
    {
        shopItem = item;
        shop = shopRef;
        playerInventory = playerInv;

        var itemData = ItemDatabase.Instance.GetItem(item.ItemId);
        if (itemData != null)
        {
            itemIcon.sprite = itemData.Icon;
            itemNameText.text = itemData.Name;
        }

        priceText.text = $"Buy: {item.BuyPrice} | Sell: {item.SellPrice}\nStock: {item.Quantity}";

        buyButton.onClick.AddListener(OnBuyClicked);
        sellButton.onClick.AddListener(OnSellClicked);
    }

    private void OnBuyClicked()
    {
        if (shop.BuyItem(shopItem.ItemId, 1, playerInventory))
        {
            Debug.Log($"Đã mua {shopItem.ItemId}");
        }
    }

    private void OnSellClicked()
    {
        if (shop.SellItem(shopItem.ItemId, 1, playerInventory))
        {
            Debug.Log($"Đã bán {shopItem.ItemId}");
        }
    }
}

using UnityEngine;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{
    [System.Serializable]
    public class ShopItem
    {
        public int ItemId;
        public int Quantity;
        public int BuyPrice;
        public int SellPrice;
    }

    [SerializeField]
    private string shopName = "Shop";

    [SerializeField]
    private List<ShopItem> shopInventory = new List<ShopItem>();

    [SerializeField]
    private int shopMoney = 10000;

    public delegate void ShopUpdateEvent();
    public event ShopUpdateEvent OnShopUpdated;

    /// <summary>
    /// Mua item từ shop
    /// </summary>
    public bool BuyItem(int itemId, int quantity, Inventory playerInventory)
    {
        var shopItem = FindShopItem(itemId);
        if (shopItem == null)
        {
            Debug.LogWarning($"Shop không có item ID: {itemId}");
            return false;
        }

        if (shopItem.Quantity < quantity)
        {
            Debug.LogWarning($"Shop không đủ {itemId}. Có: {shopItem.Quantity}, cần: {quantity}");
            return false;
        }

        long totalPrice = (long)shopItem.BuyPrice * quantity;
        if (!Currency.Instance.HasEnoughMoney(totalPrice))
        {
            Debug.LogWarning($"Không đủ tiền! Cần: {totalPrice}");
            return false;
        }

        // Thực hiện giao dịch
        var item = ItemDatabase.Instance.CreateInventoryItem(itemId);
        if (item == null) return false;

        if (playerInventory.AddItem(item, quantity))
        {
            Currency.Instance.RemoveMoney(totalPrice);
            shopItem.Quantity -= quantity;
            shopMoney += (int)totalPrice;

            Debug.Log($"Mua thành công {quantity}x {item.Name}");
            OnShopUpdated?.Invoke();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Bán item cho shop
    /// </summary>
    public bool SellItem(int itemId, int quantity, Inventory playerInventory)
    {
        if (!playerInventory.RemoveItem(itemId, quantity))
        {
            Debug.LogWarning("Không đủ item để bán");
            return false;
        }

        var shopItem = FindShopItem(itemId);
        if (shopItem == null)
        {
            // Nếu shop chưa có item này, thêm vào
            var item = ItemDatabase.Instance.CreateInventoryItem(itemId);
            if (item == null) return false;

            shopItem = new ShopItem
            {
                ItemId = itemId,
                Quantity = 0,
                BuyPrice = item.BuyPrice,
                SellPrice = item.SellPrice
            };
            shopInventory.Add(shopItem);
        }

        long totalPrice = (long)shopItem.SellPrice * quantity;
        Currency.Instance.AddMoney(totalPrice);
        shopItem.Quantity += quantity;
        shopMoney -= (int)totalPrice;

        Debug.Log($"Bán thành công {quantity}x item ID {itemId}. Nhận: {totalPrice}");
        OnShopUpdated?.Invoke();
        return true;
    }

    /// <summary>
    /// Tìm item trong shop
    /// </summary>
    private ShopItem FindShopItem(int itemId)
    {
        return shopInventory.Find(item => item.ItemId == itemId);
    }

    /// <summary>
    /// Lấy danh sách item bán
    /// </summary>
    public List<ShopItem> GetShopInventory() => shopInventory;

    /// <summary>
    /// Lấy tên shop
    /// </summary>
    public string GetShopName() => shopName;

    /// <summary>
    /// Lấy số tiền shop
    /// </summary>
    public int GetShopMoney() => shopMoney;

    /// <summary>
    /// Thêm item vào shop
    /// </summary>
    public void AddItemToShop(int itemId, int quantity, int buyPrice, int sellPrice)
    {
        var shopItem = FindShopItem(itemId);
        if (shopItem == null)
        {
            shopInventory.Add(new ShopItem
            {
                ItemId = itemId,
                Quantity = quantity,
                BuyPrice = buyPrice,
                SellPrice = sellPrice
            });
        }
        else
        {
            shopItem.Quantity += quantity;
        }
        OnShopUpdated?.Invoke();
    }
}

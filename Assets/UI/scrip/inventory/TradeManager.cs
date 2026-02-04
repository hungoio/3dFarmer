using UnityEngine;
using System.Collections.Generic;

public class TradeManager : MonoBehaviour
{
    [System.Serializable]
    public class TradeOffer
    {
        public int OfferId;
        public int ItemIdWanted;
        public int QuantityWanted;
        public int ItemIdOffered;
        public int QuantityOffered;
        public bool IsActive = true;
    }

    [SerializeField]
    private List<TradeOffer> tradeOffers = new List<TradeOffer>();

    public delegate void TradeEvent(int offerId);
    public event TradeEvent OnTradeCompleted;

    /// <summary>
    /// Tạo offer buôn bán (người chơi muốn cái này để đổi cái kia)
    /// </summary>
    public void CreateTradeOffer(int offerId, int itemWantedId, int qtyWanted, 
                                 int itemOfferedId, int qtyOffered)
    {
        var existingOffer = tradeOffers.Find(t => t.OfferId == offerId);
        if (existingOffer != null)
        {
            tradeOffers.Remove(existingOffer);
        }

        tradeOffers.Add(new TradeOffer
        {
            OfferId = offerId,
            ItemIdWanted = itemWantedId,
            QuantityWanted = qtyWanted,
            ItemIdOffered = itemOfferedId,
            QuantityOffered = qtyOffered,
            IsActive = true
        });

        Debug.Log($"Offer {offerId} tạo: {qtyWanted}x item {itemWantedId} <-> {qtyOffered}x item {itemOfferedId}");
    }

    /// <summary>
    /// Thực hiện giao dịch
    /// </summary>
    public bool ExecuteTrade(int offerId, Inventory playerInventory)
    {
        var offer = tradeOffers.Find(t => t.OfferId == offerId && t.IsActive);
        if (offer == null)
        {
            Debug.LogWarning($"Offer {offerId} không tìm thấy hoặc không hoạt động");
            return false;
        }

        // Kiểm tra player có đủ item muốn đổi
        if (playerInventory.GetItemQuantity(offer.ItemIdWanted) < offer.QuantityWanted)
        {
            Debug.LogWarning($"Không đủ item {offer.ItemIdWanted}");
            return false;
        }

        // Xóa item cần đổi
        playerInventory.RemoveItem(offer.ItemIdWanted, offer.QuantityWanted);

        // Thêm item nhận được
        var receivedItem = ItemDatabase.Instance.CreateInventoryItem(offer.ItemIdOffered);
        if (receivedItem != null)
        {
            playerInventory.AddItem(receivedItem, offer.QuantityOffered);
        }

        Debug.Log($"Giao dịch {offerId} hoàn tất!");
        OnTradeCompleted?.Invoke(offerId);
        return true;
    }

    /// <summary>
    /// Hủy offer
    /// </summary>
    public void CancelOffer(int offerId)
    {
        var offer = tradeOffers.Find(t => t.OfferId == offerId);
        if (offer != null)
        {
            offer.IsActive = false;
        }
    }

    /// <summary>
    /// Lấy offer theo ID
    /// </summary>
    public TradeOffer GetOffer(int offerId)
    {
        return tradeOffers.Find(t => t.OfferId == offerId);
    }

    /// <summary>
    /// Lấy tất cả offer hoạt động
    /// </summary>
    public List<TradeOffer> GetActiveOffers()
    {
        return tradeOffers.FindAll(t => t.IsActive);
    }
}

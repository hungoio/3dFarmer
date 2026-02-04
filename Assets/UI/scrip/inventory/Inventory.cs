using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private int inventorySize = 20;

    private List<InventorySlot> slots = new List<InventorySlot>();
    private int maxWeight = 1000;
    private int currentWeight = 0;

    public delegate void InventoryChangedEvent();
    public event InventoryChangedEvent OnInventoryChanged;

    private void Start()
    {
        InitializeInventory();
    }

    public void InitializeInventory()
    {
        slots.Clear();
        for (int i = 0; i < inventorySize; i++)
        {
            slots.Add(new InventorySlot(i));
        }
    }

    /// <summary>
    /// Thêm item vào inventory
    /// </summary>
    public bool AddItem(InventoryItem item, int quantity = 1)
    {
        if (item == null) return false;

        int remainingQuantity = quantity;

        // Thử thêm vào slot đã có item
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty && slot.Item.Id == item.Id && slot.Item.CanStack())
            {
                remainingQuantity = slot.Item.AddQuantity(remainingQuantity);
                OnInventoryChanged?.Invoke();

                if (remainingQuantity <= 0)
                    return true;
            }
        }

        // Thử thêm vào slot trống
        while (remainingQuantity > 0)
        {
            var emptySlot = slots.FirstOrDefault(s => s.IsEmpty);
            if (emptySlot == null)
            {
                Debug.LogWarning($"Inventory đầy! Chỉ thêm được {quantity - remainingQuantity} {item.Name}");
                return false;
            }

            InventoryItem newItem = new InventoryItem(item.Id, item.Name, item.Description, item.ItemType, item.MaxStackSize);
            remainingQuantity = newItem.AddQuantity(remainingQuantity);
            emptySlot.AddItem(newItem);
            OnInventoryChanged?.Invoke();
        }

        return true;
    }

    /// <summary>
    /// Xóa item khỏi inventory
    /// </summary>
    public bool RemoveItem(int itemId, int quantity = 1)
    {
        int remainingQuantity = quantity;

        foreach (var slot in slots)
        {
            if (!slot.IsEmpty && slot.Item.Id == itemId)
            {
                var removed = slot.RemoveQuantity(remainingQuantity);
                if (removed != null)
                {
                    remainingQuantity -= removed.Quantity;
                    OnInventoryChanged?.Invoke();
                }

                if (remainingQuantity <= 0)
                    return true;
            }
        }

        if (remainingQuantity > 0)
        {
            Debug.LogWarning($"Không đủ {quantity} item để xóa");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Lấy số lượng item
    /// </summary>
    public int GetItemQuantity(int itemId)
    {
        return slots.Where(s => !s.IsEmpty && s.Item.Id == itemId)
                   .Sum(s => s.Item.Quantity);
    }

    /// <summary>
    /// Lấy item từ slot
    /// </summary>
    public InventoryItem GetItemFromSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < slots.Count)
            return slots[slotIndex].Item;
        return null;
    }

    /// <summary>
    /// Lấy tất cả item
    /// </summary>
    public List<InventoryItem> GetAllItems()
    {
        return slots.Where(s => !s.IsEmpty)
                   .Select(s => s.Item)
                   .ToList();
    }

    /// <summary>
    /// Lấy số slot trống
    /// </summary>
    public int GetEmptySlotCount()
    {
        return slots.Count(s => s.IsEmpty);
    }

    /// <summary>
    /// Tìm item theo ID
    /// </summary>
    public InventoryItem FindItem(int itemId)
    {
        var slot = slots.FirstOrDefault(s => !s.IsEmpty && s.Item.Id == itemId);
        return slot?.Item;
    }

    /// <summary>
    /// Xóa toàn bộ inventory
    /// </summary>
    public void ClearInventory()
    {
        foreach (var slot in slots)
        {
            slot.ClearSlot();
        }
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// Hoán đổi hai item
    /// </summary>
    public void SwapItems(int slotIndex1, int slotIndex2)
    {
        if (slotIndex1 >= 0 && slotIndex1 < slots.Count && slotIndex2 >= 0 && slotIndex2 < slots.Count)
        {
            var temp = slots[slotIndex1].Item;
            slots[slotIndex1].Item = slots[slotIndex2].Item;
            slots[slotIndex2].Item = temp;
            OnInventoryChanged?.Invoke();
        }
    }

    /// <summary>
    /// Chuyển item từ slot này sang slot khác
    /// </summary>
    public bool MoveItem(int fromSlot, int toSlot, int quantity = 0)
    {
        if (fromSlot < 0 || fromSlot >= slots.Count || toSlot < 0 || toSlot >= slots.Count)
            return false;

        var sourceItem = slots[fromSlot].Item;
        if (sourceItem == null)
            return false;

        if (quantity <= 0)
            quantity = sourceItem.Quantity;

        // Nếu slot đích trống
        if (slots[toSlot].IsEmpty)
        {
            if (quantity >= sourceItem.Quantity)
            {
                slots[toSlot].Item = slots[fromSlot].RemoveItem();
            }
            else
            {
                var movedItem = slots[fromSlot].RemoveQuantity(quantity);
                slots[toSlot].AddItem(movedItem);
            }
            OnInventoryChanged?.Invoke();
            return true;
        }

        // Nếu cùng loại item
        if (slots[toSlot].Item.Id == sourceItem.Id && slots[toSlot].Item.CanStack())
        {
            int movedQuantity = Mathf.Min(quantity, sourceItem.Quantity);
            int remaining = slots[toSlot].Item.AddQuantity(movedQuantity);
            slots[fromSlot].Item.Quantity -= movedQuantity;

            if (slots[fromSlot].Item.Quantity <= 0)
            {
                slots[fromSlot].ClearSlot();
            }

            OnInventoryChanged?.Invoke();
            return true;
        }

        return false;
    }

    public int GetInventorySize() => inventorySize;
}

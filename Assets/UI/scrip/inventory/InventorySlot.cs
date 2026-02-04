using UnityEngine;

public class InventorySlot
{
    public int SlotIndex { get; set; }
    public InventoryItem Item { get; set; }
    public bool IsEmpty => Item == null || Item.Quantity <= 0;

    public InventorySlot(int index)
    {
        SlotIndex = index;
        Item = null;
    }

    public void AddItem(InventoryItem item)
    {
        Item = item;
    }

    public void ClearSlot()
    {
        Item = null;
    }

    public InventoryItem RemoveItem()
    {
        InventoryItem temp = Item;
        Item = null;
        return temp;
    }

    public InventoryItem RemoveQuantity(int amount)
    {
        if (IsEmpty) return null;

        if (Item.Quantity <= amount)
        {
            InventoryItem temp = Item;
            Item = null;
            return temp;
        }

        InventoryItem removedItem = new InventoryItem(
            Item.Id,
            Item.Name,
            Item.Description,
            Item.ItemType,
            Item.MaxStackSize
        )
        {
            Quantity = amount
        };

        Item.Quantity -= amount;
        return removedItem;
    }
}

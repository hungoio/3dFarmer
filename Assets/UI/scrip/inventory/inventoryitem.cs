using UnityEngine;

public class InventoryItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public string ItemType { get; set; } // "Seed", "Crop", "Tool", "Equipment"
    public Sprite Icon { get; set; }
    public int MaxStackSize { get; set; }
    
    // Hệ thống buôn bán
    public int BuyPrice { get; set; } // Giá mua từ NPC
    public int SellPrice { get; set; } // Giá bán cho NPC
    public bool IsTradeable { get; set; } // Có thể buôn bán được không

    public InventoryItem()
    {
        Quantity = 1;
        MaxStackSize = 99;
        IsTradeable = true;
    }

    public InventoryItem(int id, string name, string description, string itemType, int maxStackSize = 99)
    {
        Id = id;
        Name = name;
        Description = description;
        ItemType = itemType;
        Quantity = 1;
        MaxStackSize = maxStackSize;
        IsTradeable = true;
    }

    public override string ToString()
    {
        return $"{Name} x{Quantity}";
    }

    public bool CanStack()
    {
        return Quantity < MaxStackSize;
    }

    public int AddQuantity(int amount)
    {
        int canAdd = MaxStackSize - Quantity;
        int actualAdd = Mathf.Min(amount, canAdd);
        Quantity += actualAdd;
        return amount - actualAdd; // Trả về số lượng còn lại
    }
}

using UnityEngine;

public class ItemDatabase : ScriptableObject
{
    [System.Serializable]
    public class ItemData
    {
        public int Id;
        public string Name;
        public string Description;
        public string ItemType; // "Seed", "Crop", "Tool", "Equipment"
        public Sprite Icon;
        public int MaxStackSize;
    }

    [SerializeField]
    private ItemData[] items = new ItemData[0];

    private static ItemDatabase instance;

    public static ItemDatabase Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<ItemDatabase>("ItemDatabase");
                if (instance == null)
                {
                    Debug.LogError("ItemDatabase không tìm thấy trong Resources/ItemDatabase.asset");
                }
            }
            return instance;
        }
    }

    public ItemData GetItem(int itemId)
    {
        foreach (var item in items)
        {
            if (item.Id == itemId)
                return item;
        }
        return null;
    }

    public InventoryItem CreateInventoryItem(int itemId)
    {
        var itemData = GetItem(itemId);
        if (itemData == null)
        {
            Debug.LogWarning($"Item với ID {itemId} không tìm thấy");
            return null;
        }

        return new InventoryItem(itemData.Id, itemData.Name, itemData.Description, itemData.ItemType, itemData.MaxStackSize)
        {
            Icon = itemData.Icon
        };
    }

    public ItemData[] GetAllItems() => items;

    #if UNITY_EDITOR
    public void AddItem(ItemData item)
    {
        System.Array.Resize(ref items, items.Length + 1);
        items[items.Length - 1] = item;
    }
    #endif
}

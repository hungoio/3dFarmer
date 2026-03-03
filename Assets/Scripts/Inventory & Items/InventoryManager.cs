using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    // Kho chứa chung cho TẤT CẢ (Hạt giống, Trứng, Sữa...)
    public Dictionary<string, int> itemStorage = new Dictionary<string, int>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // (Tạm thời chưa cần Load để test cho nhanh, sau này thêm Load ở đây)
    }

    // --- 👇 HÀM BỊ THIẾU (SỬA LỖI CỦA BẠN Ở ĐÂY) 👇 ---
    public void AddItem(string itemName, int amount)
    {
        if (itemStorage.ContainsKey(itemName))
            itemStorage[itemName] += amount;
        else
            itemStorage.Add(itemName, amount);

        Debug.Log($"[Balo] Đã nhận: {amount} x {itemName}. Tổng đang có: {itemStorage[itemName]}");
    }
    // ------------------------------------------------

    public int GetItemCount(string itemName)
    {
        if (itemStorage.ContainsKey(itemName))
            return itemStorage[itemName];
        return 0;
    }

    // --- 👇 CÁC HÀM CŨ (GIỮ LẠI ĐỂ SHOP VÀ TRỒNG CÂY KHÔNG BỊ LỖI) 👇 ---

    // Hàm này cho Shop hạt giống dùng
    public void AddSeed(string cropName, int amount)
    {
        // Gọi ké hàm AddItem luôn cho gọn
        AddItem(cropName, amount);
    }

    // Hàm này cho PlantManager dùng khi trồng cây
    public bool TryUseSeed(string cropName)
    {
        if (itemStorage.ContainsKey(cropName) && itemStorage[cropName] > 0)
        {
            itemStorage[cropName]--;
            Debug.Log($"Đã dùng 1 hạt {cropName} để trồng.");
            return true;
        }
        return false;
    }

    // Hàm này cho Shop hiển thị số lượng
    public int GetSeedCount(string cropName)
    {
        return GetItemCount(cropName);
    }
}
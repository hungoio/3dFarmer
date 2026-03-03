using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    // Kho chứa chung cho TẤT CẢ (Hạt giống, Trứng, Sữa...)
    public Dictionary<string, int> itemStorage = new Dictionary<string, int>();
    [Header("Cài đặt sức chứa")]
    public int maxCapacity = 200; // Tổng số lượng tối đa mà kho có thể chứa (tính tổng tất cả các loại)
    public int upgradeLevel = 0; // Cấp độ nâng cấp hiện tại
    public int GetTotalItemCount()
    {
        int total = 0;
        foreach (var item in itemStorage.Values) total += item;
        return total;
    }
    void Awake()
    {
        Instance = this;
    }
    public int GetUpgradePrice()
    {
        // Cấp 0 -> 1000, Cấp 1 -> 2000, Cấp 2 -> 3000...
        return (upgradeLevel + 1) * 1000;
    }
    void Start()
    {
        // (Tạm thời chưa cần Load để test cho nhanh, sau này thêm Load ở đây)
    }

    // --- 👇 HÀM BỊ THIẾU (SỬA LỖI CỦA BẠN Ở ĐÂY) 👇 ---
    public void AddItem(string itemName, int amount)
    {
        // Kiểm tra xem nếu thêm vào thì có bị quá 200 không
        if (GetTotalItemCount() + amount > maxCapacity)
        {
            Debug.LogWarning("❌ Kho đã đầy! Không thể nhận thêm " + itemName);
            return; // Dừng lại, không cộng thêm đồ
        }

        if (itemStorage.ContainsKey(itemName))
            itemStorage[itemName] += amount;
        else
            itemStorage.Add(itemName, amount);

        Debug.Log($"[Balo] {itemName}: {itemStorage[itemName]}/{maxCapacity}");
    }
    public void UpgradeCapacity()
    {
        int price = GetUpgradePrice();

        // 1. Kiểm tra tiền
        if (PlayerMoney.Instance.money >= price)
        {
            // 2. Trừ tiền
            PlayerMoney.Instance.AddMoney(-price);

            // 3. Tăng sức chứa và cấp độ
            maxCapacity += 50; // Cộng thêm 50 đơn vị
            upgradeLevel++;

            Debug.Log($"✅ Nâng cấp thành công! Sức chứa mới: {maxCapacity}. Cấp tiếp theo tốn: {GetUpgradePrice()}");
        }
        else
        {
            Debug.LogWarning("❌ Bạn không đủ tiền để nâng cấp kho!");
        }
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
using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    // Dictionary để lưu: "Tên Hạt Giống" -> "Số lượng"
    public Dictionary<string, int> seedStorage = new Dictionary<string, int>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LoadInventory();
    }

    // Hàm thêm hạt giống (khi mua)
    public void AddSeed(string cropName, int amount)
    {
        if (seedStorage.ContainsKey(cropName))
            seedStorage[cropName] += amount;
        else
            seedStorage.Add(cropName, amount);

        SaveInventory(cropName);
        Debug.Log($"Đã thêm {amount} hạt {cropName}. Tổng: {seedStorage[cropName]}");
    }

    // Hàm lấy hạt giống ra dùng (khi trồng)
    public bool TryUseSeed(string cropName)
    {
        if (seedStorage.ContainsKey(cropName) && seedStorage[cropName] > 0)
        {
            seedStorage[cropName]--;
            SaveInventory(cropName); // Lưu lại ngay sau khi dùng
            return true;
        }
        return false;
    }

    // Hàm đếm số lượng hạt đang có
    public int GetSeedCount(string cropName)
    {
        if (seedStorage.ContainsKey(cropName))
            return seedStorage[cropName];
        return 0;
    }

    // --- LƯU TRỮ (Dùng PlayerPrefs) ---
    void SaveInventory(string cropName)
    {
        PlayerPrefs.SetInt("SEED_" + cropName, seedStorage[cropName]);
        PlayerPrefs.Save();
    }

    void LoadInventory()
    {
        // Duyệt qua tất cả loại cây có trong game để load số lượng
        if (PlantManager.Instance != null)
        {
            foreach (var crop in PlantManager.Instance.allCropsLibrary)
            {
                int count = PlayerPrefs.GetInt("SEED_" + crop.cropName, 0); // Mặc định là 0
                if (count > 0)
                {
                    seedStorage[crop.cropName] = count;
                }
            }
        }
    }
}
using UnityEngine;
using System.Collections.Generic;
using System.IO;

// 1. Dữ liệu Gà
[System.Serializable]
public class AnimalSaveData
{
    public string animalName;
    public float posX, posY, posZ;
}

// 2. Dữ liệu Tổng (Gồm Gà + Tiền + Balo)
[System.Serializable]
public class GameData
{
    public List<AnimalSaveData> savedAnimals = new List<AnimalSaveData>();

    // --- MỚI: TIỀN & BALO ---
    public int playerMoney;
    public List<string> inventoryItems = new List<string>(); // Tên đồ
    public List<int> inventoryCounts = new List<int>();      // Số lượng
}

public class FarmSaveManager : MonoBehaviour
{
    public static FarmSaveManager Instance;

    [Header("Thư viện loài vật")]
    public List<AnimalData> animalLibrary;

    private string savePath;

    void Awake()
    {
        Instance = this;
        savePath = Application.persistentDataPath + "/savegame.json";
    }

    void Start()
    {
        // Load trễ 0.1s để các Manager khác kịp khởi động
        Invoke("LoadGame", 0.1f);
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }

    // =========================================================
    // CHỨC NĂNG SAVE (GÀ + TIỀN + BALO)
    // =========================================================
    public void SaveGame()
    {
        GameData data = new GameData();

        // 1. LƯU GÀ
        FarmAnimalAI[] allAnimals = FindObjectsByType<FarmAnimalAI>(FindObjectsSortMode.None);
        foreach (var animal in allAnimals)
        {
            if (animal.data != null)
            {
                AnimalSaveData saveData = new AnimalSaveData();
                saveData.animalName = animal.data.animalName;
                saveData.posX = animal.transform.position.x;
                saveData.posY = animal.transform.position.y;
                saveData.posZ = animal.transform.position.z;
                data.savedAnimals.Add(saveData);
            }
        }

        // 2. LƯU TIỀN
        if (PlayerMoney.Instance != null)
        {
            data.playerMoney = PlayerMoney.Instance.money;
        }

        // 3. LƯU BALO (Tách ra 2 danh sách để lưu)
        if (InventoryManager.Instance != null)
        {
            foreach (var item in InventoryManager.Instance.itemStorage)
            {
                data.inventoryItems.Add(item.Key);   // Lưu tên (VD: Egg)
                data.inventoryCounts.Add(item.Value); // Lưu số (VD: 5)
            }
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game Saved!");
    }

    // =========================================================
    // CHỨC NĂNG LOAD (ĐÃ SỬA LỖI HIỂN THỊ BALO)
    // =========================================================
    public void LoadGame()
    {
        if (!File.Exists(savePath)) return;

        string json = File.ReadAllText(savePath);
        GameData data = JsonUtility.FromJson<GameData>(json);

        // 1. LOAD GÀ
        FarmAnimalAI[] currentAnimals = FindObjectsByType<FarmAnimalAI>(FindObjectsSortMode.None);
        foreach (var a in currentAnimals) Destroy(a.gameObject);

        foreach (var savedAnimal in data.savedAnimals)
        {
            AnimalData species = animalLibrary.Find(x => x.animalName == savedAnimal.animalName);
            if (species != null && species.animalPrefab != null)
            {
                Instantiate(species.animalPrefab, new Vector3(savedAnimal.posX, savedAnimal.posY, savedAnimal.posZ), Quaternion.identity);
            }
        }

        // 2. LOAD TIỀN
        if (PlayerMoney.Instance != null)
        {
            PlayerMoney.Instance.money = data.playerMoney;
        }

        // 3. LOAD BALO (FIX LỖI KHÔNG HIỆN UI)
        if (InventoryManager.Instance != null)
        {
            // Xóa sạch Balo cũ trước
            InventoryManager.Instance.itemStorage.Clear();

            // Dùng hàm AddItem để vừa thêm dữ liệu vừa vẽ lại UI
            for (int i = 0; i < data.inventoryItems.Count; i++)
            {
                string name = data.inventoryItems[i];
                int count = data.inventoryCounts[i];

                // 👇 QUAN TRỌNG: Dùng hàm này để UI tự cập nhật 👇
                InventoryManager.Instance.AddItem(name, count);
            }
        }

        Debug.Log("Game Loaded Successfully!");
    }

    // =========================================================
    // PHẦN LƯU CÂY (GIỮ NGUYÊN)
    // =========================================================
    public void SaveTile(LandTile tile)
    {
        string key = $"TILE_{tile.gridX}_{tile.gridZ}"; // Đảm bảo LandTile có biến gridX, gridZ

        if (tile.currentCrop != null)
        {
            string data = $"{tile.currentCrop.data.cropName}|{tile.currentCrop.PlantTimeString}";
            PlayerPrefs.SetString(key, data);
        }
        else
        {
            PlayerPrefs.DeleteKey(key);
        }
        PlayerPrefs.Save();
    }

    public void LoadTile(LandTile tile)
    {
        string key = $"TILE_{tile.gridX}_{tile.gridZ}";

        if (PlayerPrefs.HasKey(key))
        {
            string rawData = PlayerPrefs.GetString(key);
            string[] parts = rawData.Split('|');

            if (parts.Length >= 2 && PlantManager.Instance != null)
            {
                string cropName = parts[0];
                string timeString = parts[1];
                CropData cropToPlant = PlantManager.Instance.allCropsLibrary.Find(x => x.cropName == cropName);

                if (cropToPlant != null)
                {
                    GameObject newCrop = Instantiate(cropToPlant.prefab, tile.transform.position, Quaternion.identity);
                    CropInstance cropScript = newCrop.GetComponent<CropInstance>();
                    cropScript.Plant(cropToPlant);
                    cropScript.SetPlantTime(timeString);
                    tile.currentCrop = cropScript;
                }
            }
        }
    }
}
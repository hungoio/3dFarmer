using UnityEngine;

public class FarmSaveManager : MonoBehaviour
{
    public static FarmSaveManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void SaveTile(LandTile tile)
    {
        // Nếu ô đất trống -> Xóa dữ liệu cũ
        if (tile.currentCrop == null)
        {
            PlayerPrefs.DeleteKey(tile.SaveKey);
            return;
        }

        // Lưu dạng: "TenCay|ThoiGian"
        string data = tile.currentCrop.data.cropName + "|" + tile.currentCrop.PlantTimeString;
        PlayerPrefs.SetString(tile.SaveKey, data);
        PlayerPrefs.Save();
    }

    // HÀM MỚI THÊM VÀO ĐỂ LOAD
    public void LoadTile(LandTile tile)
    {
        if (!PlayerPrefs.HasKey(tile.SaveKey)) return;

        string data = PlayerPrefs.GetString(tile.SaveKey);
        string[] splitData = data.Split('|'); // Tách chuỗi ra

        string cropName = splitData[0];
        string timeString = splitData[1];

        // 1. Tìm loại cây dựa trên tên
        CropData cropType = PlantManager.Instance.GetCropByName(cropName);

        if (cropType != null)
        {
            // 2. Trồng lại cây vào ô đất
            PlantManager.Instance.SpawnCrop(tile, cropType, timeString);
        }
    }
}
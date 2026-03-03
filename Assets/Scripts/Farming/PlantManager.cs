using UnityEngine;
using System.Collections.Generic;

public class PlantManager : MonoBehaviour
{
    public static PlantManager Instance;
    public CropData selectedCrop;

    // KÉO TẤT CẢ CROP DATA (Lúa, Ngô...) VÀO LIST NÀY Ở INSPECTOR
    public List<CropData> allCropsLibrary;

    void Awake()
    {
        Instance = this;
    }

    public void PlantCrop(LandTile tile)
    {
        if (selectedCrop == null || !tile.IsEmpty()) return;

        // 👇 KIỂM TRA TÚI ĐỒ Ở ĐÂY 👇
        // Nếu dùng thử hạt giống thành công (nghĩa là kho > 0 và đã trừ 1)
        if (InventoryManager.Instance.TryUseSeed(selectedCrop.cropName))
        {
            SpawnCrop(tile, selectedCrop, System.DateTime.Now.ToString());
            FarmSaveManager.Instance.SaveTile(tile);

            Debug.Log("Đã trồng: " + selectedCrop.cropName);
        }
        else
        {
            Debug.Log("🚫 HẾT HẠT GIỐNG RỒI! Hãy đi mua thêm.");
            // Gợi ý: Bạn có thể hiện thông báo UI "Hết hạt giống" ở đây
        }
    }

    // Hàm này dùng chung cho cả việc Trồng Mới và Load Game
    public void SpawnCrop(LandTile tile, CropData cropData, string timeString)
    {
        GameObject cropObj = Instantiate(
            cropData.prefab,
            tile.transform.position + Vector3.up * 0.5f,
            Quaternion.identity
        );

        CropInstance crop = cropObj.AddComponent<CropInstance>();
        crop.Plant(cropData); // Set dữ liệu cơ bản
        crop.SetPlantTime(timeString); // Set thời gian đã lưu

        tile.currentCrop = crop;
    }

    // Hàm tìm kiếm dữ liệu cây theo tên
    public CropData GetCropByName(string name)
    {
        foreach (var c in allCropsLibrary)
        {
            if (c.cropName == name) return c;
        }
        return null;
    }
}
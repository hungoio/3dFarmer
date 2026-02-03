using UnityEngine;
using System;

public class FarmSaveManager : MonoBehaviour
{
    public static FarmSaveManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void SaveTile(LandTile tile)
    {
        if (tile.currentCrop == null)
        {
            PlayerPrefs.DeleteKey(GetKey(tile.tileID));
            return;
        }

        string cropName = tile.currentCrop.data.cropName;
        string plantTime = DateTime.Now.ToString("O");

        string value = cropName + "|" + plantTime;
        PlayerPrefs.SetString(GetKey(tile.tileID), value);
        PlayerPrefs.Save();
    }

    public void LoadTile(LandTile tile, CropData[] allCrops)
    {
        string key = GetKey(tile.tileID);
        if (!PlayerPrefs.HasKey(key)) return;

        string value = PlayerPrefs.GetString(key);
        string[] parts = value.Split('|');

        string cropName = parts[0];
        string plantTime = parts[1];

        CropData cropData = FindCrop(cropName, allCrops);
        if (cropData == null) return;

        GameObject obj = Instantiate(
            cropData.prefab,
            tile.transform.position + Vector3.up * 0.5f,
            Quaternion.identity
        );

        CropInstance crop = obj.AddComponent<CropInstance>();
        crop.Plant(cropData);
        crop.SetPlantTime(plantTime);

        tile.currentCrop = crop;
    }

    string GetKey(int tileID)
    {
        return "TILE_" + tileID;
    }

    CropData FindCrop(string name, CropData[] crops)
    {
        foreach (var crop in crops)
            if (crop.cropName == name)
                return crop;
        return null;
    }
}

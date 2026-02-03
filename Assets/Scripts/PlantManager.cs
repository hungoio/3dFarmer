using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public static PlantManager Instance;

    public CropData selectedCrop;

    void Awake()
    {
        Instance = this;
    }

    public void PlantCrop(LandTile tile)
    {
        if (selectedCrop == null)
            return;

        if (!tile.IsEmpty())
            return;

        GameObject cropObj = Instantiate(
            selectedCrop.prefab,
            tile.transform.position + Vector3.up * 0.5f,
            Quaternion.identity
        );

        CropInstance crop = cropObj.AddComponent<CropInstance>();
        crop.Plant(selectedCrop);

        tile.currentCrop = crop;
        FarmSaveManager.Instance.SaveTile(tile);

    }

}

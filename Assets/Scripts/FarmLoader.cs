using UnityEngine;

public class FarmLoader : MonoBehaviour
{
    public LandTile[] tiles;
    public CropData[] allCrops;

    void Start()
    {
        foreach (var tile in tiles)
        {
            FarmSaveManager.Instance.LoadTile(tile, allCrops);
        }
    }
}

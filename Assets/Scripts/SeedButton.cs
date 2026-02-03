using UnityEngine;
using UnityEngine.UI;

public class SeedButton : MonoBehaviour
{
    public CropData cropData;
    public Image icon;

    public void SelectSeed()
    {
        PlantManager.Instance.selectedCrop = cropData;
    }
}

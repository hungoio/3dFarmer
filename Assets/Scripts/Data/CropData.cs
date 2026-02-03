using UnityEngine;

[CreateAssetMenu(menuName = "Farm/Crop")]
public class CropData : ScriptableObject
{
    public string cropName;
    public float growTime;      // thời gian lớn (giây)
    public GameObject prefab;   // model cây
    public int sellPrice;
}

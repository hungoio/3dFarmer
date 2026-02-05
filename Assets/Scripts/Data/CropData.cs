using UnityEngine;

[CreateAssetMenu(menuName = "Farm/Crop")]
public class CropData : ScriptableObject
{
    public string cropName;
    public float growTime;      // thời gian lớn (giây)
    public GameObject prefab;   // model cây

    // Bạn có thể giữ sellPrice ở đây nếu muốn, 
    // nhưng thường giá bán sẽ nằm bên ItemData (nông sản)
    public int sellPrice;

    public int buyPrice;  // Giá mua hạt giống

    // 👇👇 THÊM 2 DÒNG NÀY VÀO ĐỂ SỬA LỖI 👇👇
    [Header("Thông tin Thu Hoạch")]
    public ItemData productItem; // Cây này sẽ rớt ra Item gì? (Ví dụ: Cà rốt Item)
    public int yieldAmount = 1;  // Số lượng rớt ra (Ví dụ: 1)
}
using UnityEngine;

[CreateAssetMenu(menuName = "Farm/Item")]
public class ItemData : ScriptableObject
{
    public string itemName; // Tên vật phẩm (Ví dụ: "Egg")
    public Sprite icon;     // Hình ảnh sẽ hiện trong balo
    public int sellPrice;   // Giá bán (Ví dụ: 10 đồng)
}
using UnityEngine;

[CreateAssetMenu(menuName = "Farm/Animal")]
public class AnimalData : ScriptableObject
{
    public string animalName; // Tên (Gà, Bò...)
    public GameObject productPrefab; // Sản phẩm (Trứng, Sữa...)
    public float produceTime; // Thời gian đẻ (giây)
    public int buyPrice; // Giá mua
    public GameObject animalPrefab;
}
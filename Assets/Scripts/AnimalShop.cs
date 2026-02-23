using UnityEngine;

public class AnimalShop : MonoBehaviour
{
    public AnimalData animalToBuy; // Kéo file ChickenData vào đây
    public Transform spawnPoint;   // Kéo cái AnimalSpawnPoint vào đây

    // Hàm này sẽ gắn vào nút Mua
    public void BuyAnimal()
    {
        if (animalToBuy == null) return;

        int cost = animalToBuy.buyPrice;

        // Kiểm tra tiền
        if (PlayerMoney.Instance.money >= cost)
        {
            // 1. Trừ tiền
            PlayerMoney.Instance.AddMoney(-cost);

            // 2. Sinh ra con vật tại điểm Spawn
            Instantiate(animalToBuy.animalPrefab, spawnPoint.position, Quaternion.identity);

            Debug.Log("Đã mua một con " + animalToBuy.animalName);
        }
        else
        {
            Debug.Log("Không đủ tiền mua gà rồi! Cần: " + cost);
        }
    }
}
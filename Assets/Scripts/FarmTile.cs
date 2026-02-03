using UnityEngine;

public class FarmTile : MonoBehaviour
{
    // Prefab cây sẽ trồng
    public GameObject plantPrefab;

    // Cây hiện đang trồng trên ô đất (nếu có)
    private Plant currentPlant;

    // Hàm được gọi khi player trồng cây
    public void PlantSeed()
    {
        // Nếu đã có cây rồi thì không trồng nữa
        if (currentPlant != null) return;

        // Tạo cây tại vị trí ô đất
        GameObject plantObj = Instantiate(
            plantPrefab,
            transform.position + Vector3.up * 0.1f,
            Quaternion.identity
        );

        // Lấy script Plant từ prefab
        currentPlant = plantObj.GetComponent<Plant>();
    }

    // Kiểm tra xem có thể thu hoạch chưa
    public bool CanHarvest()
    {
        return currentPlant != null && currentPlant.IsReady();
    }

    // Thu hoạch cây
    public void Harvest()
    {
        if (!CanHarvest()) return;

        currentPlant.Harvest();
        currentPlant = null;
    }
}

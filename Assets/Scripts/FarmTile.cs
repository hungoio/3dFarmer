using UnityEngine;

public class FarmTile : MonoBehaviour
{
    // Prefab cây sẽ trồng
    public GameObject plantPrefab;

    // Reference tới Inventory
    [SerializeField]
    private Inventory inventory;

    // Cây hiện đang trồng trên ô đất (nếu có)
    private Plant currentPlant;

    private void Start()
    {
        // Tự động tìm Inventory nếu chưa được gán
        if (inventory == null)
        {
            inventory = FindObjectOfType<Inventory>();
        }
    }

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
        
        // Truyền inventory cho cây để nó có thể thêm item khi thu hoạch
        if (currentPlant != null && inventory != null)
        {
            currentPlant.SetInventory(inventory);
        }
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

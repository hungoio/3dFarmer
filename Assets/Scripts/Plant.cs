using UnityEngine;

public class Plant : MonoBehaviour
{
    // Thời gian để cây lớn (giây)
    public float growTime = 5f;

    // Item cây sẽ drop khi thu hoạch
    [SerializeField]
    private int harvestItemId = 1;
    [SerializeField]
    private string harvestItemName = "Tomato";
    [SerializeField]
    private string harvestItemDescription = "A fresh tomato";
    [SerializeField]
    private string harvestItemType = "Vegetable";
    [SerializeField]
    private int harvestQuantity = 1;
    
    // Hình ảnh cây - sẽ dùng làm icon trong kho đồ
    [SerializeField]
    private Sprite plantSprite;

    private Inventory inventory;
    private SpriteRenderer spriteRenderer;
    float timer;
    bool isReady;

    private void Start()
    {
        // Lấy SpriteRenderer từ GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Nếu plantSprite chưa được gán, lấy từ SpriteRenderer
        if (plantSprite == null && spriteRenderer != null)
        {
            plantSprite = spriteRenderer.sprite;
        }

        // Tìm Inventory từ scene (nếu chưa được gán)
        if (inventory == null)
        {
            var inventoryObj = FindObjectOfType<Inventory>();
            if (inventoryObj != null)
                inventory = inventoryObj;
        }
    }

    void Update()
    {
        // Nếu cây đã lớn rồi thì không cần update nữa
        if (isReady) return;

        timer += Time.deltaTime;

        if (timer >= growTime)
        {
            isReady = true;
            OnGrown();
        }
    }

    // Khi cây trưởng thành
    void OnGrown()
    {
        // Ví dụ: cây to hơn
        transform.localScale *= 1.2f;
    }

    // Kiểm tra có thu hoạch được không
    public bool IsReady()
    {
        return isReady;
    }

    // Set inventory tham chiếu
    public void SetInventory(Inventory inv)
    {
        inventory = inv;
    }

    // Thu hoạch cây
    public void Harvest()
    {
        if (inventory != null)
        {
            // Tạo item với thông tin harvest
            InventoryItem harvestItem = new InventoryItem(
                harvestItemId,
                harvestItemName,
                harvestItemDescription,
                harvestItemType,
                maxStackSize: 50
            );
            
            // Dùng sprite của cây làm icon
            if (plantSprite != null)
            {
                harvestItem.Icon = plantSprite;
            }

            // Thêm vào inventory
            bool added = inventory.AddItem(harvestItem, harvestQuantity);
            if (added)
                Debug.Log($"Thu hoạch {harvestQuantity}x {harvestItemName}");
            else
                Debug.LogWarning($"Không thể thêm {harvestItemName} vào inventory");
        }
        else
        {
            Debug.LogWarning("Inventory không được thiết lập cho Plant");
        }

        // Xóa cây
        Destroy(gameObject);
    }
}

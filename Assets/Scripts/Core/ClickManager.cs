using UnityEngine;

public class ClickManager : MonoBehaviour
{
    private LandTile currentTile;

    [Header("Giao diện Công cụ")]
    public GameObject toolPopup;       // Ô dành cho Rổ (Trứng)
    public GameObject shearToolPopup;  // Ô dành cho Cái Kéo (Lông cừu)

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // 1. Kiểm tra xem có đang trong chế độ đặt cây táo không
                if (GameManager.Instance != null && GameManager.Instance.isPlacingTree)
                {
                    // Kiểm tra nếu click trúng mặt đất (Hãy gắn Tag "Ground" cho sàn của bạn)
                    if (hit.collider.CompareTag("Ground"))
                    {
                        Instantiate(GameManager.Instance.appleTreePrefab, hit.point, Quaternion.identity);

                        PlayerMoney.Instance.AddMoney(-GameManager.Instance.appleTreePrice); // Trừ tiền khi đặt cây
                        // Đặt xong thì tắt chế độ đặt cây để quay về bình thường
                        GameManager.Instance.isPlacingTree = false;
                        return; // Thoát ra để không click trúng cái khác
                    }
                }

                // 2. Logic thu hoạch cây táo (Mới thêm)
                AppleTree tree = hit.collider.GetComponent<AppleTree>();
                if (tree != null)
                {
                    tree.Harvest(); // Gọi hàm thu hoạch táo
                    return;
                }
                // 1. KIỂM TRA CLICK VÀO CỪU (MỚI THÊM)
                Shearable sheep = hit.collider.GetComponent<Shearable>();
                if (sheep != null && sheep.GetComponent<FarmAnimalAI>().isReadyToHarvest)
                {
                    if (shearToolPopup != null)
                    {
                        shearToolPopup.transform.position = Input.mousePosition;
                        shearToolPopup.SetActive(true);
                    }
                    return; // Bấm trúng cừu rồi thì thoát
                }

                // 2. XỬ LÝ NHẶT ĐỒ (TRỨNG/SỮA)
                Collectable item = hit.collider.GetComponent<Collectable>();
                if (item != null)
                {
                    if (toolPopup != null)
                    {
                        toolPopup.transform.position = Input.mousePosition;
                        toolPopup.SetActive(true);
                    }
                    return;
                }

                // 3. CLICK VÀO CHUỒNG GÀ
                ChickenCoop coop = hit.collider.GetComponent<ChickenCoop>();
                if (coop != null)
                {
                    coop.OnCoopClicked();
                    return;
                }

                // 4. XỬ LÝ ĐẤT TRỒNG CÂY
                LandTile tile = hit.collider.GetComponent<LandTile>();
                if (tile == null) return;

                if (currentTile != null)
                    currentTile.Deselect();

                currentTile = tile;
                currentTile.Select();

                if (tile.IsEmpty())
                {
                    PlantManager.Instance.PlantCrop(tile);
                }
                else if (tile.currentCrop.IsReady())
                {
                    PlayerMoney.Instance.AddMoney(tile.currentCrop.data.sellPrice);
                    Destroy(tile.currentCrop.gameObject);
                    tile.currentCrop = null;
                    FarmSaveManager.Instance.SaveTile(tile);
                }
            }
        }
    }
}
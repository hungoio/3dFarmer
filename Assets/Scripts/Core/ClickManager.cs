using UnityEngine;

public class ClickManager : MonoBehaviour
{
    private LandTile currentTile;

    // 👇 THÊM DÒNG NÀY: Kéo cái ToolPopup ở Bước 2 vào đây
    public GameObject toolPopup;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // --- XỬ LÝ NHẶT ĐỒ (TRỨNG/SỮA) ---
                Collectable item = hit.collider.GetComponent<Collectable>();
                if (item != null)
                {
                    // THAY VÌ GỌI item.Collect() NHƯ CŨ, CHÚNG TA LÀM THẾ NÀY:
                    if (toolPopup != null)
                    {
                        // Dời Menu Rổ tới vị trí chuột và Bật nó lên
                        toolPopup.transform.position = Input.mousePosition;
                        toolPopup.SetActive(true);
                    }
                    return; // Dừng, không kiểm tra đất trồng cây nữa
                }

                // --- XỬ LÝ ĐẤT TRỒNG CÂY (Giữ nguyên code cũ của bạn) ---
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
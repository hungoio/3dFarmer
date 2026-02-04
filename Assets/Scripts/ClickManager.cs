using UnityEngine;

public class ClickManager : MonoBehaviour
{
    private LandTile currentTile;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                LandTile tile = hit.collider.GetComponent<LandTile>();
                if (tile == null) return;

                if (currentTile != null)
                    currentTile.Deselect();

                currentTile = tile;
                currentTile.Select();

                // TRỒNG
                if (tile.IsEmpty())
                {
                    PlantManager.Instance.PlantCrop(tile);
                }
                // THU HOẠCH
                else if (tile.currentCrop.IsReady())
                {
                    // cộng tiền
                    PlayerMoney.Instance.AddMoney(
                        tile.currentCrop.data.sellPrice
                    );

                    Destroy(tile.currentCrop.gameObject);
                    tile.currentCrop = null;
                    FarmSaveManager.Instance.SaveTile(tile);
                }

            }
        }
    }
}

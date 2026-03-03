using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public GameObject basketToolPopup; // UI Cái Rổ
    public GameObject shearToolPopup;  // UI Cái Kéo (Mới)

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // 1. Kiểm tra Cừu
                Shearable sheep = hit.collider.GetComponent<Shearable>();
                if (sheep != null && sheep.GetComponent<FarmAnimalAI>().isReadyToHarvest)
                {
                    ShowTool(shearToolPopup);
                    return;
                }

                // 2. Kiểm tra Vật phẩm rơi (Trứng)
                Collectable item = hit.collider.GetComponent<Collectable>();
                if (item != null)
                {
                    ShowTool(basketToolPopup);
                    return;
                }
            }
        }
    }

    void ShowTool(GameObject tool)
    {
        if (tool != null)
        {
            tool.transform.position = Input.mousePosition;
            tool.SetActive(true);
        }
    }
}
using UnityEngine;

public class Shearable : MonoBehaviour
{
    private FarmAnimalAI ai;

    void Start()
    {
        // Tự động tìm script AI gắn cùng trên con cừu
        ai = GetComponent<FarmAnimalAI>();
    }

    // Hàm này sẽ được "Cái Kéo" (HarvestDragTool) gọi khi bạn kéo đi ngang qua
    public void Collect()
    {
        // Chỉ cho phép cắt lông nếu con cừu đang ở trạng thái sẵn sàng (đang ngồi chờ)
        if (ai != null && ai.isReadyToHarvest)
        {
            ai.OnHarvested(); // Gọi lệnh cho cừu đứng dậy và tạo ra lông
            Debug.Log("Đã cắt lông cừu thành công!");
        }
    }
}
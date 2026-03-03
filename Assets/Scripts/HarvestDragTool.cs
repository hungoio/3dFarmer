using UnityEngine;
using UnityEngine.EventSystems; // Cần cái này để dùng UI Drag

public class HarvestDragTool : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 originalPosition;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        // Nhớ vị trí gốc của cái rổ để khi thả tay ra nó bay về chỗ cũ
        originalPosition = transform.position;

        // Thêm CanvasGroup để xử lý Raycast
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // TẮT chặn tia Raycast của UI. 
        // Mục đích: Để tia sáng có thể xuyên qua cái Rổ và chạm vào quả Trứng 3D bên dưới.
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 1. Cái rổ di chuyển theo ngón tay / con trỏ chuột
        transform.position = Input.mousePosition;

        // 2. Bắn tia quét xuống mặt đất 3D
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Kiểm tra xem tia có cọ trúng vật phẩm nào không
            Collectable item = hit.collider.GetComponent<Collectable>();
            if (item != null)
            {
                // Nếu cọ trúng -> Thu hoạch luôn!
                item.Collect();
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Khi thả tay ra: Trả rổ về vị trí cũ, bật lại Raycast, ẩn Menu đi
        transform.position = originalPosition;
        canvasGroup.blocksRaycasts = true;

        // Ẩn cái GameObject cha (ToolPopup)
        transform.parent.gameObject.SetActive(false);
    }
}
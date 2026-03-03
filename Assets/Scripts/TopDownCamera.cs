using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    [Header("Cài đặt Zoom")]
    public float zoomSpeed = 20f;
    public float minHeight = 5f;  // Zoom gần nhất (độ cao thấp nhất)
    public float maxHeight = 25f; // Zoom xa nhất (độ cao cao nhất)

    private Camera cam;

    // Mặt phẳng ảo nằm ngang ở tọa độ Y = 0 (tương đương mặt đất)
    private Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
    private Vector3 dragOrigin;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        HandlePan();
        HandleZoom();
    }

    void HandlePan()
    {
        // Dùng chuột PHẢI (1) hoặc chuột GIỮA (2) để kéo bản đồ
        // (Không dùng chuột trái (0) để tránh bị trùng với lúc click thu hoạch/trồng cây)
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (groundPlane.Raycast(ray, out float enter))
            {
                // Ghi nhớ điểm trên mặt đất lúc vừa bấm chuột
                dragOrigin = ray.GetPoint(enter);
            }
        }

        if (Input.GetMouseButton(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (groundPlane.Raycast(ray, out float enter))
            {
                // Tính toán khoảng cách chuột đã di chuyển
                Vector3 currentPoint = ray.GetPoint(enter);
                Vector3 difference = dragOrigin - currentPoint;

                // Kéo camera đi theo khoảng cách đó
                transform.position += difference;
            }
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            // Trượt camera tiến/lùi dọc theo hướng nó đang nhìn
            Vector3 move = cam.transform.forward * scroll * zoomSpeed;
            Vector3 newPos = transform.position + move;

            // Chặn không cho zoom quá gần hoặc quá xa
            if (newPos.y >= minHeight && newPos.y <= maxHeight)
            {
                transform.position = newPos;
            }
        }
    }
}
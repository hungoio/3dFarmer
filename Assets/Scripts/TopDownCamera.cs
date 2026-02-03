using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    // Player mà camera sẽ theo
    public Transform target;

    // Khoảng cách camera so với player
    // Y cao để nhìn từ trên xuống
    // Z âm để nhìn hơi chéo
    public Vector3 offset = new Vector3(0f, 10f, -5f);

    // Tốc độ camera theo player (càng lớn càng nhanh)
    public float followSpeed = 10f;

    void LateUpdate()
    {
        if (target == null) return;

        // Vị trí mong muốn của camera
        Vector3 desiredPosition = target.position + offset;

        // Di chuyển camera mượt tới vị trí mong muốn
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );
    }
}

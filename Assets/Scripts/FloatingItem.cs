using UnityEngine;
using TMPro; // Dùng TextMeshPro

public class FloatingItem : MonoBehaviour
{
    public SpriteRenderer iconRenderer;
    public TextMeshPro textMesh;

    public float moveSpeed = 1.5f;
    public float lifeTime = 1.0f;

    void Update()
    {
        // 1. Bay lên từ từ
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // 2. Đếm ngược để tự hủy
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Hàm cài đặt thông tin (được gọi ngay khi sinh ra)
    public void Setup(Sprite icon, int amount)
    {
        if (iconRenderer != null) iconRenderer.sprite = icon;
        if (textMesh != null) textMesh.text = "+" + amount;
    }
}
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public ItemData data; // Kéo cái thẻ EggData vào đây

    // Hàm này tự động chạy khi bạn Click chuột vào vật thể
    void OnMouseDown()
    {
        Debug.Log("Đã click vào trứng!");

        if (InventoryManager.Instance != null)
        {
            // 1. Gửi trứng vào kho (Balo)
            InventoryManager.Instance.AddItem(data.itemName, 1);

            // 2. Xóa quả trứng trên đất đi (Nhặt rồi thì phải biến mất)
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("Chưa có InventoryManager trong Scene nên không nhặt được!");
        }
    }
}
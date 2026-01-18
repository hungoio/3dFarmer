using UnityEngine;

public class Plant : MonoBehaviour
{
    // Thời gian để cây lớn (giây)
    public float growTime = 5f;

    float timer;
    bool isReady;

    void Update()
    {
        // Nếu cây đã lớn rồi thì không cần update nữa
        if (isReady) return;

        timer += Time.deltaTime;

        if (timer >= growTime)
        {
            isReady = true;
            OnGrown();
        }
    }

    // Khi cây trưởng thành
    void OnGrown()
    {
        // Ví dụ: cây to hơn
        transform.localScale *= 1.2f;
    }

    // Kiểm tra có thu hoạch được không
    public bool IsReady()
    {
        return isReady;
    }

    // Thu hoạch cây
    public void Harvest()
    {
        // Sau này có thể spawn item ở đây
        Destroy(gameObject);
    }
}

using UnityEngine;

public class CropInstance : MonoBehaviour
{
    public CropData data;

    private float plantTime;
    private float growProgress; // 0 → 1

    void Update()
    {
        if (data == null) return;

        float elapsed = Time.time - plantTime;
        growProgress = Mathf.Clamp01(elapsed / data.growTime);

        // scale từ nhỏ → lớn
        float scale = Mathf.Lerp(0.2f, 1f, growProgress);
        transform.localScale = Vector3.one * scale;
    }

    public void Plant(CropData cropData)
    {
        data = cropData;
        plantTime = Time.time;
        transform.localScale = Vector3.one * 0.2f;
    }

    public bool IsReady()
    {
        return growProgress >= 1f;
    }
}

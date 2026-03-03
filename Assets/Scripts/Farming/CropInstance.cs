using UnityEngine;
using UnityEngine.UI;
using System; // <-- Cần cái này để dùng DateTime

public class CropInstance : MonoBehaviour
{
    public CropData data;
    public Image progressBar;

    private float growProgress;

    // --- 👇 PHẦN BỊ THIẾU ĐÃ ĐƯỢC THÊM VÀO 👇 ---
    private DateTime plantTime; // Biến lưu thời gian thực

    // Property này chuyển đổi thời gian sang chuỗi để lưu vào file Save
    public string PlantTimeString => plantTime.ToString("O");
    // ---------------------------------------------

    void Update()
    {
        if (data == null) return;

        UpdateGrowProgress();

        // Scale cây lớn dần
        float scale = Mathf.Lerp(0.2f, 1f, growProgress);
        transform.localScale = Vector3.one * scale;

        if (progressBar != null)
            progressBar.fillAmount = growProgress;
    }

    void UpdateGrowProgress()
    {
        // Tính toán thời gian trôi qua dựa trên biến plantTime
        double elapsedSeconds = (DateTime.Now - plantTime).TotalSeconds;

        growProgress = Mathf.Clamp01(
            (float)(elapsedSeconds / data.growTime)
        );
    }

    // Hàm gọi khi TRỒNG MỚI
    public void Plant(CropData cropData)
    {
        data = cropData;
        plantTime = DateTime.Now; // Lấy giờ hiện tại

        growProgress = 0f;
        transform.localScale = Vector3.one * 0.2f;
    }

    // Hàm gọi khi LOAD GAME
    public void SetPlantTime(string timeString)
    {
        // Chuyển chuỗi (từ file save) ngược lại thành DateTime
        if (!string.IsNullOrEmpty(timeString))
        {
            plantTime = DateTime.Parse(timeString);
        }
        else
        {
            plantTime = DateTime.Now;
        }
    }

    public bool IsReady()
    {
        return growProgress >= 1f;
    }
}
using UnityEngine;
using UnityEngine.UI;
using System;

public class CropInstance : MonoBehaviour
{
    public CropData data;
    public Image progressBar;

    private float growProgress;

    // ⏱ THỜI GIAN TRỒNG (dạng string để save)
    private string plantTimeString;

    void Start()
    {
        // khi load lại game
        if (!string.IsNullOrEmpty(plantTimeString))
            UpdateGrowProgress();
    }

    void Update()
    {
        if (data == null) return;

        UpdateGrowProgress();

        // scale cây
        float scale = Mathf.Lerp(0.2f, 1f, growProgress);
        transform.localScale = Vector3.one * scale;

        if (progressBar != null)
            progressBar.fillAmount = growProgress;
    }

    void UpdateGrowProgress()
    {
        DateTime plantTime = DateTime.Parse(plantTimeString);
        double elapsedSeconds = (DateTime.Now - plantTime).TotalSeconds;

        growProgress = Mathf.Clamp01(
            (float)(elapsedSeconds / data.growTime)
        );
    }

    public void Plant(CropData cropData)
    {
        data = cropData;

        // lưu thời điểm trồng
        plantTimeString = DateTime.Now.ToString();

        growProgress = 0f;
        transform.localScale = Vector3.one * 0.2f;
    }
    public void SetPlantTime(string timeString)
    {
        plantTimeString = timeString;
    }

    public bool IsReady()
    {
        return growProgress >= 1f;
    }
}

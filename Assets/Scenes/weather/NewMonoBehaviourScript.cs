using UnityEngine;
using System.Collections;

public class RainController : MonoBehaviour
{
    [Header("Cấu hình xác suất")]
    [Range(0f, 1f)]
    public float spawnChance = 0.3f; // 30% tỉ lệ mưa

    [Header("Thời gian chờ giữa các lần kiểm tra")]
    public float minDelay = 20f;     // thời gian chờ tối thiểu
    public float maxDelay = 40f;     // thời gian chờ tối đa

    [Header("Thời gian mưa")]
    public float rainDuration = 20f; // mưa kéo dài bao lâu

    [Header("Hiệu ứng mưa")]
    public GameObject rainEffect;    // Kéo Particle System vào đây

    private bool isRaining = false;

    void Start()
    {
        if (rainEffect == null)
        {
            Debug.LogWarning("Chưa gán rainEffect!");
            return;
        }

        rainEffect.SetActive(false);
        StartCoroutine(RainRoutine());
    }

    IEnumerator RainRoutine()
    {
        while (true)
        {
            // Random thời gian chờ
            float randomDelay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(randomDelay);

            // Nếu đang mưa thì bỏ qua (chống chồng)
            if (isRaining)
                continue;

            // Random xác suất
            if (Random.value < spawnChance)
            {
                StartCoroutine(StartRain());
            }
            else
            {
                Debug.Log("Không có mưa lần này.");
            }
        }
    }

    IEnumerator StartRain()
    {
        isRaining = true;

        rainEffect.SetActive(true);
        Debug.Log("🌧 Mưa đã bắt đầu!");

        yield return new WaitForSeconds(rainDuration);

        rainEffect.SetActive(false);
        isRaining = false;

        Debug.Log("☀ Mưa đã kết thúc!");
    }
}
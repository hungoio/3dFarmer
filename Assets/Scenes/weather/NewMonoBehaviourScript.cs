using UnityEngine;
using System.Collections;

public class RainController : MonoBehaviour
{
    [Header("Cấu hình trong Inspector")]
    [Range(0f, 1f)]
    public float spawnChance = 0.3f; // Tỉ lệ xuất hiện (30%)
    public float delayTime = 30f;    // Thời gian chờ trước khi kiểm tra
    public float rainDuration = 20f; // Thời gian mưa bật

    public GameObject rainEffect;    // Prefab hoặc Particle System mưa

    void Start()
    {
        StartCoroutine(RainRoutine());
    }

    IEnumerator RainRoutine()
    {
        // Chờ delayTime giây
        yield return new WaitForSeconds(delayTime);

        // Kiểm tra ngẫu nhiên
        if (Random.value < spawnChance)
        {
            rainEffect.SetActive(true); // bật mưa
            Debug.Log("Mưa đã xuất hiện!");

            // Sau rainDuration giây thì tắt mưa
            yield return new WaitForSeconds(rainDuration);
            rainEffect.SetActive(false);
            Debug.Log("Mưa đã tắt!");
        }
        else
        {
            Debug.Log("Không có mưa lần này.");
        }
    }
}

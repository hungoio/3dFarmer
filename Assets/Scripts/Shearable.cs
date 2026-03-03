using UnityEngine;

public class Shearable : MonoBehaviour
{
    private FarmAnimalAI ai;

    void Start()
    {
        ai = GetComponent<FarmAnimalAI>();
    }

    // Hàm này được HarvestDragTool (Cái kéo) gọi khi quét qua
    public void Collect()
    {
        if (ai != null && ai.isReadyToHarvest)
        {
            ai.OnHarvested();
            Debug.Log("Cắt lông thành công!");
        }
    }
}
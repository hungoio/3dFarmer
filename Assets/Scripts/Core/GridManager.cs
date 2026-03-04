using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 5;
    public int height = 5;
    public GameObject landTilePrefab;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        Vector3 origin = transform.position;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 pos = origin + new Vector3(x, 0, z);

                GameObject tileObj = Instantiate(
                    landTilePrefab,
                    pos,
                    Quaternion.identity,
                    transform
                );

                tileObj.name = $"Tile_{x}_{z}";

                // Lấy script LandTile
                LandTile tileScript = tileObj.GetComponent<LandTile>();

                if (tileScript != null)
                {
                    // 1. GÁN TỌA ĐỘ (Quan trọng để tạo SaveKey đúng)
                    tileScript.gridX = x;
                    tileScript.gridZ = z;

                    // 2. LOAD DỮ LIỆU NGAY LẬP TỨC
                    // (Vì FarmSaveManager là Singleton nên gọi lúc nào cũng được)
                    FarmSaveManager.Instance.LoadTile(tileScript);
                }
            }
        }
    }
}
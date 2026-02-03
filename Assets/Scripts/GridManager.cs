using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 5;
    public int height = 5;
    public GameObject landTilePrefab;

    private LandTile selectedTile;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GameObject tileObj = Instantiate(
                    landTilePrefab,
                    new Vector3(x, 0, z),
                    Quaternion.identity
                );

                tileObj.name = $"Tile_{x}_{z}";
            }
        }
    }
}

using UnityEngine;

public class LandTile : MonoBehaviour
{
    public int tileID; // 👈 GÁN TRONG INSPECTOR

    private Renderer rend;
    private Color defaultColor;
    public int gridX;
    public int gridZ;

    public CropInstance currentCrop;
    public string SaveKey => $"Tile_{gridX}_{gridZ}";

    void Awake()
    {
        rend = GetComponent<Renderer>();
        rend.material = new Material(rend.material);
        defaultColor = rend.material.color;
    }

    public void Select()
    {
        rend.material.color = Color.yellow;
    }

    public void Deselect()
    {
        rend.material.color = defaultColor;
    }

    public bool IsEmpty()
    {
        return currentCrop == null;
    }
}

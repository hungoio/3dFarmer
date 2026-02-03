using UnityEngine;

public class LandTile : MonoBehaviour
{
    private Renderer rend;
    private Color defaultColor;

    public CropInstance currentCrop;

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

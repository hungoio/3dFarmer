using UnityEngine;

public class LandTile : MonoBehaviour
{
    private Renderer rend;
    private Color defaultColor;

    void Start()
    {
        rend = GetComponent<Renderer>();
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
}

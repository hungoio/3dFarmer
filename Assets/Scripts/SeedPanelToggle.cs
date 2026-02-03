using UnityEngine;

public class SeedPanelToggle : MonoBehaviour
{
    public GameObject seedPanel;

    void Start()
    {
        seedPanel.SetActive(false); // đảm bảo tắt lúc đầu
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            seedPanel.SetActive(!seedPanel.activeSelf);
        }
    }
}

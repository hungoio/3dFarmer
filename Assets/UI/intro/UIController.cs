using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject[] uiPanels;
    private int currentIndex = 0;

    void Start()
    {
        ShowPanel(currentIndex);
    }

    public void NextUI()
    {
        currentIndex++;
        if (currentIndex >= uiPanels.Length)
        {
            currentIndex = 0;
        }
        ShowPanel(currentIndex);
    }

    void ShowPanel(int index)
    {
        for (int i = 0; i < uiPanels.Length; i++)
        {
            uiPanels[i].SetActive(i == index);
        }
    }
}

using UnityEngine;

public class ShopToggle1 : MonoBehaviour
{
    public GameObject panelToToggle; // Kéo AnimalShopPanel vào đây

    void Update()
    {
        // Ví dụ bấm phím P (Pet) để mở shop thú cưng
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (panelToToggle != null)
                panelToToggle.SetActive(!panelToToggle.activeSelf);
        }
    }

    // Hàm này dùng cho nút X (Đóng)
    public void CloseShop()
    {
        if (panelToToggle != null) panelToToggle.SetActive(false);
    }
}

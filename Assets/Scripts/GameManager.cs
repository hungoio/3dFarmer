using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton để các script khác (như ClickManager) dễ dàng gọi tới
    public static GameManager Instance;

    [Header("Cài đặt Cây Táo")]
    public bool isPlacingTree = false; // Trạng thái đang cầm cây để đặt
    public GameObject appleTreePrefab; // Kéo Prefab cây táo vào đây
    public int appleTreePrice = 100; // 👈 Thêm dòng này: Giá 1 cây táo là 100$
    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // Hàm này sẽ được gọi khi bạn bấm nút "Mua Cây Táo" trên UI
    public void OnClickBuyAppleTree()
    {
        if(PlayerMoney.Instance.money >= appleTreePrice)
        {
            isPlacingTree = true;
            Debug.Log("Đã bật chế độ đặt cây táo. Hãy click vào mặt đất!");
        }
        else
        {
            Debug.LogWarning("Bạn không đủ tiền mua cây táo!");
        }
    }
}
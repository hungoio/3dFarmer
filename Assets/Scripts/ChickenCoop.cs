using UnityEngine;

public class ChickenCoop : MonoBehaviour
{
    [Header("Cài đặt Chuồng")]
    public AnimalData chickenData; // Kéo file ChickenData vào đây
    public int maxCapacity = 4;    // Tối đa 4 con
    public Transform spawnPoint;   // Điểm sinh ra gà (nằm trong chuồng)

    [Header("Trạng thái hiện tại")]
    public int currentChickens = 0; // Số gà đang có

    [Header("UI Liên kết")]
    public GameObject buyPopupUI;   // Kéo cái Menu có nút Mua Gà vào đây

    void Start()
    {
        // Ẩn Menu đi khi mới vào game
        if (buyPopupUI != null) buyPopupUI.SetActive(false);
    }

    // Hàm này được ClickManager gọi khi người chơi bấm vào Chuồng
    public void OnCoopClicked()
    {
        if (currentChickens < maxCapacity)
        {
            // Mở bảng mua gà
            buyPopupUI.SetActive(true);
        }
        else
        {
            Debug.Log("Chuồng đã đầy 4/4 con! Không thể nuôi thêm.");
            // Ở đây sau này có thể làm dòng chữ bay lên "Chuồng đã đầy!"
        }
    }

    // Hàm này sẽ gắn vào NÚT "MUA" trên UI
    public void BuyChicken()
    {
        int cost = chickenData.buyPrice;

        // Kiểm tra xem còn chỗ không và có đủ tiền không
        if (currentChickens < maxCapacity && PlayerMoney.Instance.money >= cost)
        {
            // 1. Trừ tiền
            PlayerMoney.Instance.AddMoney(-cost);

            // 2. Sinh ra con gà tại SpawnPoint
            GameObject newChicken = Instantiate(chickenData.animalPrefab, spawnPoint.position, Quaternion.identity);
            // 2. Lấy script AI của nó và giao vị trí chuồng (homePosition)
            FarmAnimalAI ai = newChicken.GetComponent<FarmAnimalAI>();
            if (ai != null)
            {
                // Giao cho nó vị trí của SpawnPoint làm tâm điểm đi dạo
                ai.SetHome(spawnPoint.position);
            }

            // 3. Tăng số lượng gà trong chuồng lên 1
            currentChickens++;

            Debug.Log($"Đã mua gà! Số gà hiện tại: {currentChickens}/{maxCapacity}");

            // Nếu mua xong mà chuồng đầy luôn thì TẮT bảng UI đi
            if (currentChickens >= maxCapacity)
            {
                buyPopupUI.SetActive(false);
            }
        }
        else if (PlayerMoney.Instance.money < cost)
        {
            Debug.Log("Không đủ tiền mua gà!");
        }
    }

    // Hàm để nút "X" trên UI gọi để đóng bảng
    public void ClosePopup()
    {
        buyPopupUI.SetActive(false);
    }
}
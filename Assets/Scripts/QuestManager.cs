using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("UI References")]
    public GameObject questPanel;
    public TMP_Text quest1Text, quest2Text, quest3Text;

    // Trạng thái hoàn thành nhiệm vụ để tránh nhận thưởng nhiều lần
    private bool q1Done, q2Done, q3Done;

    void Awake() { Instance = this; }

    void Update()
    {
        if (questPanel.activeSelf) UpdateQuestUI();
    }

    public void UpdateQuestUI()
    {
        // Nhiệm vụ 1: 10 Quả táo
        int appleCount = GetItemCount("Apple");
        quest1Text.text = $"Thu thập 10 quả táo: {appleCount}/10";
        quest1Text.color = appleCount >= 10 ? Color.green : Color.white;

        // Nhiệm vụ 2: 20 Quả trứng
        int eggCount = GetItemCount("Egg");
        quest2Text.text = $"Thu thập 20 quả trứng: {eggCount}/20";
        quest2Text.color = eggCount >= 20 ? Color.green : Color.white;

        // Nhiệm vụ 3: Kho lên cấp 2 (upgradeLevel = 2)
        int storageLv = InventoryManager.Instance.upgradeLevel;
        quest3Text.text = $"Nâng cấp kho lên cấp 2: {storageLv}/2";
        quest3Text.color = storageLv >= 2 ? Color.green : Color.white;
    }

    int GetItemCount(string name)
    {
        if (InventoryManager.Instance.itemStorage.ContainsKey(name))
            return InventoryManager.Instance.itemStorage[name];
        return 0;
    }

    // Hàm gọi khi nhấn nút "Nhận thưởng" trên mỗi nhiệm vụ
    public void ClaimQuest1()
    {
        if (!q1Done && GetItemCount("Apple") >= 10)
        {
            PlayerMoney.Instance.AddMoney(100);
            q1Done = true;
            Debug.Log("Nhận 100 đồng từ nhiệm vụ Táo!");
        }
    }

    public void ClaimQuest2()
    {
        if (!q2Done && GetItemCount("Egg") >= 20)
        {
            PlayerMoney.Instance.AddMoney(150);
            q2Done = true;
            Debug.Log("Nhận 150 đồng từ nhiệm vụ Trứng!");
        }
    }

    public void ClaimQuest3()
    {
        if (!q3Done && InventoryManager.Instance.upgradeLevel >= 2)
        {
            PlayerMoney.Instance.AddMoney(400);
            q3Done = true;
            Debug.Log("Nhận 400 đồng từ nhiệm vụ Nâng cấp kho!");
        }
    }
}
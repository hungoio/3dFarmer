using UnityEngine;

public class Currency : MonoBehaviour
{
    [SerializeField]
    private long initialMoney = 1000; // Set tiền ban đầu trong Inspector
    
    private long money = 0;
    private long maxMoney = 999999999; // Tối đa 999,999,999

    public delegate void MoneyChangedEvent(long newAmount);
    public event MoneyChangedEvent OnMoneyChanged;

    private static Currency instance;

    public static Currency Instance
    {
        get
        {
            if (instance == null)
                instance = FindFirstObjectByType<Currency>();
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            money = initialMoney;
        }
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Thêm tiền
    /// </summary>
    public void AddMoney(long amount)
    {
        if (amount <= 0) return;

        long newAmount = System.Math.Min(money + amount, maxMoney);
        long actualAdd = newAmount - money;
        money = newAmount;
        OnMoneyChanged?.Invoke(money);

        Debug.Log($"Thêm {actualAdd} tiền. Tổng: {money}");
    }

    /// <summary>
    /// Trừ tiền
    /// </summary>
    public bool RemoveMoney(long amount)
    {
        if (amount <= 0) return false;
        if (money < amount)
        {
            Debug.LogWarning($"Không đủ tiền! Hiện có: {money}, cần: {amount}");
            return false;
        }

        money -= amount;
        OnMoneyChanged?.Invoke(money);
        Debug.Log($"Bớt {amount} tiền. Còn lại: {money}");
        return true;
    }

    /// <summary>
    /// Lấy số tiền hiện tại
    /// </summary>
    public long GetMoney() => money;

    /// <summary>
    /// Thiết lập số tiền
    /// </summary>
    public void SetMoney(long amount)
    {
        money = System.Math.Min(amount, maxMoney);
        OnMoneyChanged?.Invoke(money);
    }

    /// <summary>
    /// Kiểm tra có đủ tiền không
    /// </summary>
    public bool HasEnoughMoney(long amount)
    {
        return money >= amount;
    }

    /// <summary>
    /// Định dạng tiền hiển thị
    /// </summary>
    public string FormatMoney(long amount)
    {
        if (amount >= 1000000)
            return (amount / 1000000f).ToString("F2") + "M";
        if (amount >= 1000)
            return (amount / 1000f).ToString("F2") + "K";
        return amount.ToString();
    }
}

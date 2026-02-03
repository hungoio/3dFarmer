using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    public static PlayerMoney Instance;

    public int money = 0;
    private const string MONEY_KEY = "PLAYER_MONEY";

    void Awake()
    {
        Instance = this;

        // LOAD TIỀN KHI VÀO GAME
        money = PlayerPrefs.GetInt(MONEY_KEY, 0);
        Debug.Log("💰 Load money: " + money);
    }

    public void AddMoney(int amount)
    {
        money += amount;

        // SAVE NGAY KHI CỘNG
        PlayerPrefs.SetInt(MONEY_KEY, money);
        PlayerPrefs.Save();

        Debug.Log("💰 Money hiện tại: " + money);
    }
}

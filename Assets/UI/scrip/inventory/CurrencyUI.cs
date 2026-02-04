using UnityEngine;
using TMPro;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI moneyText;

    [SerializeField]
    private string prefix = "Money: ";

    [SerializeField]
    private string suffix = " Gold";

    private void Start()
    {
        if (Currency.Instance != null)
        {
            Currency.Instance.OnMoneyChanged += UpdateMoneyDisplay;
            UpdateMoneyDisplay(Currency.Instance.GetMoney());
        }
        else
        {
            Debug.LogError("Currency Instance không tìm thấy!");
        }
    }

    private void UpdateMoneyDisplay(long money)
    {
        if (moneyText != null)
        {
            moneyText.text = prefix + Currency.Instance.FormatMoney(money) + suffix;
        }
    }

    private void OnDestroy()
    {
        if (Currency.Instance != null)
            Currency.Instance.OnMoneyChanged -= UpdateMoneyDisplay;
    }
}

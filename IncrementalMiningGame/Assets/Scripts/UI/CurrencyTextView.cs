using TMPro;
using UnityEngine;

public class CurrencyTextView : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private TextMeshProUGUI currencyText;

    private void Awake()
    {
        currencyText.text = "";
    }

    private void Start()
    {
        EconomyManager.Instance.OnCurrencyCalculated += ShowCurrency;
    }

    private void OnDisable()
    {
        EconomyManager.Instance.OnCurrencyCalculated -= ShowCurrency;
    }

    private void ShowCurrency()
    {
        panel.SetActive(true);

        currencyText.text = $"{EconomyManager.Instance.MainCurrency}";
    }
}

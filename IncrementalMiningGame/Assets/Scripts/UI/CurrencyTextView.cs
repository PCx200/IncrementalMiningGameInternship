using TMPro;
using UnityEngine;

public class CurrencyTextView : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private TextMeshProUGUI currencyText;

    [SerializeField]
    private CurrencyData currencyData;

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

        currencyText.text = $"{EconomyManager.Instance.GetValueOfCurrency(currencyData)}";
    }
}

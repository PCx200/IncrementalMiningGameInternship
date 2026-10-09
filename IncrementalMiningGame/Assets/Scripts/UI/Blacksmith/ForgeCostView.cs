using TMPro;
using UnityEngine;

public class ForgeCostView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI costText;

    [SerializeField]
    private CurrencyData currencyDataRequired;

    [SerializeField]
    private BlacksmithManager blacksmithManager;

    private void OnEnable()
    {
        EconomyManager.Instance.OnCurrencyCalculated += Show;
    }

    private void OnDisable()
    {
        EconomyManager.Instance.OnCurrencyCalculated -= Show;
    }

    private void Start()
    {
        Show();
    }

    private void Show()
    {
        costText.text = $"{EconomyManager.Instance.GetValueOfCurrency(currencyDataRequired)} / {blacksmithManager.ForgeCost}";
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;

    private ResourceBag resourceBag;

    private Dictionary<CurrencyData, int> currencies = new();

    public event Action OnCurrencyCalculated;
    public event Action<CurrencyData, int> OnCurrencyChanged;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        RoundManager.Instance.OnRoundEnd += AddProfit;
    }

    private void OnDisable()
    {
        RoundManager.Instance.OnRoundEnd -= AddProfit;
    }

    public void SetResourceBag(ResourceBag resourceBag)
    {
        this.resourceBag = resourceBag;
    }

    public int GetValueOfCurrency(CurrencyData currencyData)
    {
        if (currencyData == null)
        { 
            return 0;
        }

        return currencies.GetValueOrDefault(currencyData, 0);
    }

    public Dictionary<CurrencyData, int> CalculateProfit()
    {
        Dictionary<CurrencyData, int> profit = new();

        foreach (var block in resourceBag.GetBlocks())
        {
            BlockData blockData = block.Key;
            int blockCount = block.Value;

            CurrencyData currencyData = blockData.Currency;
            int value = blockData.Value;

            int totalValue = blockCount * value;

            if (profit.ContainsKey(currencyData))
            {
                profit[currencyData] += totalValue;
            }
            else
            {
                profit.Add(currencyData, totalValue);
            }
        }
            return profit;
    }

    public void AddCurrency(CurrencyData currencyData, int amount)
    {
        if (currencyData == null)
        { 
            return;
        }

        if (amount == 0)
        {         
            return;
        }  

        if (currencies.ContainsKey(currencyData))
        {
            currencies[currencyData] += amount;
        }
        else
        {
            currencies.Add(currencyData, amount);
        }

        OnCurrencyChanged?.Invoke(currencyData, currencies[currencyData]);
    }

    public void AddProfit()
    {
        Dictionary<CurrencyData, int> profit = CalculateProfit();

        foreach (var entry in profit)
        {
            AddCurrency(entry.Key, entry.Value);
        }

        OnCurrencyCalculated?.Invoke();
    }

    public bool TrySpendCurrency(CurrencyData currency, int amount)
    {
        if (currency == null || amount <= 0)
        { 
            return false;
        }

        int currentAmount = GetValueOfCurrency(currency);

        if (currentAmount < amount)
        { 
            return false;
        }

        currencies[currency] -= amount;

        OnCurrencyChanged?.Invoke(currency, currencies[currency]);
        OnCurrencyCalculated?.Invoke();

        return true;
    }
}

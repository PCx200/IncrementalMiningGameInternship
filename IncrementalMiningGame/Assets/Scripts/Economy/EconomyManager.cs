using System;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;

    private Inventory inventory;

    [SerializeField]
    private int mainCurrency;
    public int MainCurrency => mainCurrency;

    public event Action OnCurrencyCalculated;

    private void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        inventory = FindFirstObjectByType<Inventory>();
        RoundManager.Instance.OnRoundEnd += AddProfit;
    }

    private void OnDisable()
    {
        RoundManager.Instance.OnRoundEnd -= AddProfit;
    }

    public int CalculateProfit()
    {
        int profit = 0;

        foreach (var block in inventory.GetBlocks())
        {
            int blockValue = block.Key.Value;
            int count = block.Value;

            profit += blockValue * count;
        }

        return profit;
    }

    public void AddProfit()
    {
        mainCurrency += CalculateProfit();

        OnCurrencyCalculated?.Invoke();
    }
}

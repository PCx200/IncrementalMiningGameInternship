using System;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;

    private ResourceBag resourceBag;

    [SerializeField]
    private int mainCurrency;
    public int MainCurrency => mainCurrency;

    public event Action OnCurrencyCalculated;

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

    public void SetInventory(ResourceBag inventory)
    {
        this.resourceBag = inventory;
    }

    public int CalculateProfit()
    {

        if (resourceBag == null)
        {
            return 0;
        }

        int profit = 0;

        foreach (var block in resourceBag.GetBlocks())
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

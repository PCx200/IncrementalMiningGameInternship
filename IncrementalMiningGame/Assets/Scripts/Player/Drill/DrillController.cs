using System;
using UnityEngine;

public class DrillController : MonoBehaviour
{
    [SerializeField]
    private DrillData data;
    public DrillData Data => data;

    private DrillStatManager drillStatManager;
    public DrillStatManager DrillStatManager => drillStatManager;

    [SerializeField]
    private float currentFuel;
    public float CurrentFuel => currentFuel;

    public event Action OnTankEmpty;

    [SerializeField]
    private BlockDamagedChannel blockDamagedChannel;

    [SerializeField]
    private BlockDamagedChannel blockDamagedPenaltyChannel;

    private void Awake()
    {
        drillStatManager = new DrillStatManager(data);

        drillStatManager.OnStatsChanged += HandleStatsChanged;
    }

    private void Start()
    {
        currentFuel = drillStatManager.GetValue(DrillStat.FuelTankCapacity);


        RoundManager.Instance.RegisterDrill(this);
    }

    private void Update()
    {
        DrainFuel();
    }

    private void OnEnable()
    {
        blockDamagedChannel.Raised += DrainFuelAfterMining;
        blockDamagedPenaltyChannel.Raised += DrainFuelAfterMining;
    }

    private void OnDisable()
    {
        blockDamagedChannel.Raised -= DrainFuelAfterMining;
        blockDamagedPenaltyChannel.Raised -= DrainFuelAfterMining;


    }

    private void OnDestroy()
    {
        drillStatManager.OnStatsChanged -= HandleStatsChanged;
    }

    private void DrainFuel()
    {
        float fuelConsumption = drillStatManager.GetValue(DrillStat.FuelConsumptionPerSecond);

        currentFuel -= fuelConsumption * Time.deltaTime;

        CheckTankEmpty();
    }

    private void DrainFuelAfterMining(float amount)
    {
        currentFuel -= amount;

        CheckTankEmpty();
    }

    private void CheckTankEmpty()
    {
        if (currentFuel <= 0f)
        {
            currentFuel = 0f;
            OnTankEmpty?.Invoke();
        }
    }

    private void HandleStatsChanged()
    {
        float maximumFuel = drillStatManager.GetValue(DrillStat.FuelTankCapacity);

        if (currentFuel > maximumFuel)
        {
            currentFuel = maximumFuel;
        }
    }
}

using System;
using UnityEngine;

public class DrillController : MonoBehaviour
{
    [SerializeField]
    private DrillData data;
    public DrillData Data => data;

    [SerializeField]
    private float currentFuel;
    public float CurrentFuel => currentFuel;

    private event Action OnTankEmpty;

    [SerializeField]
    private BlockDamagedChannel blockDamagedChannel;

    [SerializeField]
    private BlockDamagedChannel blockDamagedPenaltyChannel;

    private void Start()
    {
        currentFuel = data.FuelTankCapacity;
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

    private void DrainFuel()
    {
        currentFuel -= data.FuelConsumptionPerSecond * Time.deltaTime;

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
}

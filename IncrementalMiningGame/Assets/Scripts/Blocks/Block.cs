using System;
using UnityEngine;

public class Block : MonoBehaviour, IDamageable
{
    [SerializeField] 
    private BlockData data;
    public BlockData Data => data;

    private float currentHealth;
    private int currentValue;

    private void Awake()
    {
        currentHealth = data.Health;
        currentValue = data.Value;
    }

    public void TakeDamage(float flatDamage)
    {
        currentHealth -= flatDamage;

        data.BlockDamagedChannel?.Raise(data.FuelConsumption);

        if (data.Health / 4 > flatDamage)
        {
            data.BlockDamagedPenaltyChannel?.Raise(data.FuelPenalty);
        }

        if (currentHealth <= 0.0f)
        {
            data.BlockDestroyChannel?.Raise(data);
            Destroy(gameObject);
        }
    }
}

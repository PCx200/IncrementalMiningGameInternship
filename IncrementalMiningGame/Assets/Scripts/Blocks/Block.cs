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

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        data.BlockDamagedChannel?.Raise(data.FuelConsumption);

        float hitsRequired = 4.0f;

        if (data.Health / hitsRequired > damage)
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

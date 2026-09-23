using System;
using UnityEngine;

public class Block : MonoBehaviour, IDamageable
{
    [SerializeField] private BlockData data;
    public BlockData Data => data;

    private float currentHealth;
    private int currentValue;

    public event Action OnDamaged;

    private void Awake()
    {
        currentHealth = data.Health;
        currentValue = data.Value;
    }

    public void TakeDamage(float flatDamage)
    {
        currentHealth -= flatDamage;

        OnDamaged?.Invoke();

        Debug.Log($"{this.name} health left: {currentHealth}");

        if (currentHealth <= 0.0f)
        {
            data.BlockDestroyChannel?.Raise(data);
            Destroy(gameObject);
        }
    }
}

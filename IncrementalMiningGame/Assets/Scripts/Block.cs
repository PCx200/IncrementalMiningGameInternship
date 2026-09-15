using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private BlockData data;

    public BlockData Data => data;

    private float currentHealth;
    private int currentValue;
    private float currentSpawnRate;
}

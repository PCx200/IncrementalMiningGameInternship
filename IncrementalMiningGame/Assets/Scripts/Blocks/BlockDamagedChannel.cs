using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockDamagedChannel", menuName = "Scriptable Objects/BlockDamagedChannel")]
public class BlockDamagedChannel : ScriptableObject
{
    public event Action<float> Raised;

    public void Raise(float fuelConsumed)
    {
        Raised?.Invoke(fuelConsumed);
    }
}

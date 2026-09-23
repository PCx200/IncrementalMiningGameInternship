using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockDestroyChannel", menuName = "Scriptable Objects/BlockDestroyChannel")]
public class BlockDestroyChannel : ScriptableObject
{
    public event Action<BlockData> Raised;

    public void Raise(BlockData data)
    {
        Raised?.Invoke(data);
    }
}

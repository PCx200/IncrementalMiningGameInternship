using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockData", menuName = "Scriptable Objects/BlockData")]
public class BlockData : ScriptableObject
{
    public float Health;
    public int Value;
    public float Weight;
    public float SpawnRate;

    public List<int> Layers = new();

}

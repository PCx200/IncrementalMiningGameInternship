using System.Collections.Generic;
using UnityEngine;
public enum BlockGenerationType
{
    Base,
    StoneVariant,
    Ore
}

[CreateAssetMenu(fileName = "BlockData", menuName = "Scriptable Objects/BlockData")]
public class BlockData : ScriptableObject
{
    public float Health;
    public int Value;
    public float Weight;

    [Header("Generation")]
    public BlockGenerationType GenerationType;

    [Range(0,1f)]public float SpawnRate;
    public int MinClusterSize;

    public int MaxClusterSize;

    public int MinClusterCount;
    public int MaxClusterCount;

    public List<int> Layers = new();

}

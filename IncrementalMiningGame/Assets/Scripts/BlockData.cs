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
    public Block Prefab;

    public float Health;
    public int Value;
    public float Weight;

    [Header("Generation")]
    public BlockGenerationType GenerationType;

    [Range(0,1f)] public float SpawnRate;

    public int MinClusterSize;
    public int MaxClusterSize;

    public int MinClusterCount;
    public int MaxClusterCount;

    [Header("Noise Based")]

    [Range(0f, 1f), Tooltip("The minimal value and ore can spread, based on the noise value")] 
    public float MinNoiseValue;

    [Range(0f, 1f), Tooltip("The maximal value and ore can spread, based on the noise value")] 
    public float MaxNoiseValue;
}

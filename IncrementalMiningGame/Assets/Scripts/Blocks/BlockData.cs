using TMPro;
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
    [Header("Visualisation")]
    public Block Prefab;
    public TMP_SpriteAsset Sprite;

    [Header("Stats")]
    public float Health;
    public int Value;
    public float Weight;

    [Header("Fuel-related")]
    public float FuelConsumption;
    public float FuelPenalty;

    [Header("Generation")]
    public BlockGenerationType GenerationType;

    public int MinClusterSize;
    public int MaxClusterSize;

    public int MinClusterCount;
    public int MaxClusterCount;

    [Header("Event Channels")]
    public BlockDestroyChannel BlockDestroyChannel;
    public BlockDamagedChannel BlockDamagedChannel;
    public BlockDamagedChannel BlockDamagedPenaltyChannel;
}

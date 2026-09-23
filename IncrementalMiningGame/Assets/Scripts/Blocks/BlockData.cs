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

    public int MinClusterSize;
    public int MaxClusterSize;

    public int MinClusterCount;
    public int MaxClusterCount;

    public BlockDestroyChannel BlockDestroyChannel;
}

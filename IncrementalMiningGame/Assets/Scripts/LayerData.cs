using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LayerData", menuName = "Scriptable Objects/LayerData")]
public class LayerData : ScriptableObject
{
    public Block BaseBlock;
    public List<Block> StoneVariants = new();
    public List<Block> Ores = new();
}

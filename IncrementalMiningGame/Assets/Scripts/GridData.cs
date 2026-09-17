using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridData", menuName = "Scriptable Objects/GridData")]
public class GridData : ScriptableObject
{
    public int Width;
    public int Depth;
    public List<int> LayersDepth;

}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class SkillData : ScriptableObject
{
    public string ID;
    public Sprite Icon;

    public string Name;

    [TextArea]
    public string Description;

    public int Cost;

    public float CostMultiplier;

    public int MaxLevel;
    
    public float BaseMultiplier;
    
    public List<float> MultiplierPerLevel;
    
    public List<SkillPrerequisite> Prerequisites = new();

    public SkillEffect Effect;
}

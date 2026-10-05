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

    public CurrencyData Currency;
    public int Cost;

    public float CostMultiplier;

    public int MaxLevel;

    public List<SkillPrerequisite> Prerequisites = new();

    public SkillOutput SkillOutput;
}

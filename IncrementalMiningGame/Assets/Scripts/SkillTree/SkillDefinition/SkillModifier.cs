using UnityEngine;

[CreateAssetMenu(fileName = "SkillModifier", menuName = "Scriptable Objects/SkillModifier")]
public class SkillModifier : SkillOutput
{
    [Tooltip("ID is used for what stat we modify")]   
    public string StatID;

    public ModifierOperation ModifierOperation;

    public float ValuePerLevel;

    public float GetValue(int skillLevel)
    {
        return ValuePerLevel * skillLevel;
    }
}

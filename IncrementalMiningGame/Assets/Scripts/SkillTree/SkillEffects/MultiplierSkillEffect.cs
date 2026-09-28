using UnityEngine;

[CreateAssetMenu(fileName = "MultiplierSkillEffect", menuName = "Scriptable Objects/MultiplierSkillEffect")]
public class MultiplierSkillEffect : SkillEffect
{
    public override void Apply(PlayerController player, Skill skill)
    {
        float multiplier = skill.GetMultiplier();
    }
}

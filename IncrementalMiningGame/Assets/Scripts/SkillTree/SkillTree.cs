using System;
using System.Collections.Generic;

public static class SkillTree
{
    private static Dictionary<SkillData, Skill> skills = new();
    public static Dictionary<SkillData, Skill> Skills => skills;

    public static event Action OnSkillLeveled;

    public static void AddSkill(Skill skill)
    {
        skills[skill.Data] = skill;
    }

    public static Skill GetSkill(SkillData skill)
    {
        return skills[skill];
    }

    public static void SkillLeveled()
    {
        OnSkillLeveled?.Invoke();
    }

    public static void Clear()
    {
        skills.Clear();
    }
}

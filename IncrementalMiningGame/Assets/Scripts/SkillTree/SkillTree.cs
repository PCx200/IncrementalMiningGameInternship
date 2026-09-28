using System.Collections.Generic;

public static class SkillTree
{
    private static Dictionary<SkillData, Skill> skills = new();

    public static void AddSkill(Skill skill)
    {
        skills[skill.Data] = skill;
    }

    public static Skill GetSkill(SkillData skill)
    {
        return skills[skill];
    }

    public static void Clear()
    {
        skills.Clear();
    }
}

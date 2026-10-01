using System;
using System.Collections.Generic;

public class SkillTree
{
    private Dictionary<SkillData, Skill> skills = new();

    public event Action<Skill> OnSkillLeveled;

    public void RegisterSkill(Skill skill)
    {
        if (skill == null || skill.Data == null)
        {
            return;
        }

        skills[skill.Data] = skill;
    }

    public void UnregisterSkill(Skill skill)
    {
        if (skill == null || skill.Data == null)
        {
            return;
        }

        if (skills.TryGetValue(skill.Data, out Skill registeredSkill) && registeredSkill == skill)
        {
            skills.Remove(skill.Data);
        }
    }

    public bool TryGetSkill(SkillData skillData, out Skill skill)
    {
        return skills.TryGetValue(skillData, out skill);
    }

    public bool TryLevelUp(Skill skill)
    {
        //needs logic

        if (skill == null)
        {
            return false;
        }

        if (skill.IsMaxed())
        {
            return false;
        }

        if (!skill.IsUnlocked())
        {
            return false;
        }

        //reduce the cost from the economy manager
        int cost = skill.GetCost();

        skill.SetLevel(skill.CurrentLevel + 1);

        OnSkillLeveled.Invoke(skill);

        return true;
    }

    public void Clear()
    {
        skills.Clear();
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree
{
    private Dictionary<SkillData, Skill> skills = new();
    public Dictionary<SkillData, Skill> Skills => skills;

    private Dictionary<SkillData, int> levels = new();

    public event Action<Skill> OnSkillLeveled;

    public void RegisterSkill(Skill skill)
    {
        if (skill == null || skill.Data == null || skills.ContainsKey(skill.Data))
        {
            return;
        }

        skills[skill.Data] = skill;

        if (!levels.ContainsKey(skill.Data))
        {
            levels[skill.Data] = 0;
        }
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

    public int GetLevel(SkillData skillData)
    {
        if (skillData == null)
        {
            return 0;
        }

        return levels.TryGetValue(skillData, out int level) ? level : 0;
    }

    public bool TryLevelUp(Skill skill)
    {
        if (skill == null)
        {
            return false;
        }

        if (skill.CurrentLevel >= skill.Data.MaxLevel)
        {
            return false;
        }

        if (!skill.IsUnlocked())
        {
            return false;
        }

        int cost = skill.GetCost();
        int currentValue = EconomyManager.Instance.GetValueOfCurrency(skill.Data.Currency);

        if (!EconomyManager.Instance.TrySpendCurrency(skill.Data.Currency, cost))
        {
            return false;
        }

        SetLevel(skill.Data, skill.CurrentLevel + 1);

        return true;
    }

    public int GetCost(Skill skill)
    {
        return Mathf.RoundToInt(skill.Data.Cost * Mathf.Pow(skill.Data.CostMultiplier, skill.CurrentLevel));
    }

    public bool IsUnlocked(Skill skill)
    {
        if (skill.Data.Prerequisites == null || skill.Data.Prerequisites.Count == 0)
        {
            return true;
        }

        foreach (var skillPrerequisite in skill.Data.Prerequisites)
        {
            if (!TryGetSkill(skillPrerequisite.SkillData, out Skill requiredSkill))
            {
                return false;
            }

            if (requiredSkill.CurrentLevel < skillPrerequisite.RequiredLevel)
            {
                return false;
            }
        }

        return true;
    }

    public bool HasAnyUnlockedPrerequisite(Skill skill)
    {
        if (skill.Data.Prerequisites == null || skill.Data.Prerequisites.Count == 0)
        {
            return true;
        }

        foreach (var skillPrerequisite in skill.Data.Prerequisites)
        {
            if (!TryGetSkill(skillPrerequisite.SkillData, out Skill requiredSkill))
            {
                continue;
            }

            if (requiredSkill.CurrentLevel > 0)
            {
                return true;
            }
        }

        return false;
    }

    public void SetLevel(SkillData skillData, int level)
    {
        if (skillData == null)
        {
            return;
        }

        level = Mathf.Clamp(level, 0, skillData.MaxLevel);

        levels[skillData] = level;

        if (skills.TryGetValue(skillData, out Skill skill))
        {
            OnSkillLeveled?.Invoke(skill);
        }
    }

    public SkillOutput GetOutput(SkillData skillData)
    {
        return skillData?.SkillOutput;
    }

    public IReadOnlyDictionary<SkillData, int> GetProgression()
    {
        return levels;
    }


    public void Clear()
    {
        skills.Clear();
        levels.Clear();
    }
}

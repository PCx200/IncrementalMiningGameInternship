using System;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField]
    private SkillData data;
    public SkillData Data => data;

    [SerializeField]
    private int currentLevel;
    public int CurrentLevel => currentLevel;

    public Skill(SkillData data)
    {
        this.data = data;
        currentLevel = 0;
    }

    private void Awake()
    {
        SkillTree.AddSkill(this);
    }

    public bool IsMaxed()
    {
        return currentLevel >= data.MaxLevel;
    }

    public void LevelUp()
    {
        if (IsMaxed())
        {
            return;
        }
        
        currentLevel++;

        SkillTree.SkillLeveled();
    }

    public int GetCost()
    {
        return Mathf.RoundToInt(data.Cost * Mathf.Pow(data.CostMultiplier, currentLevel));
    }

    public float GetMultiplier()
    {
        if (IsMaxed())
        {
            return data.BaseMultiplier + data.MultiplierPerLevel[data.MaxLevel - 1];
        }

        return data.BaseMultiplier + data.MultiplierPerLevel[currentLevel];
    }

    public bool IsUnlocked()
    {
        if (data.Prerequisites == null || data.Prerequisites.Count == 0)
        {
            return true;
        }

        foreach (var skillPrerequisite in data.Prerequisites)
        {
            Skill requiredSkill = SkillTree.GetSkill(skillPrerequisite.SkillData);

            if (requiredSkill.CurrentLevel < skillPrerequisite.RequiredLevel)
            {
                return false;
            }
        }

        return true;
    }

    public bool HasAnyUnlockedPrerequisite()
    {
        if (data.Prerequisites == null || data.Prerequisites.Count == 0)
        {
            return true;
        }

        foreach (var skillPrerequisite in data.Prerequisites)
        {
            Skill requiredSkill = SkillTree.GetSkill(skillPrerequisite.SkillData);

            if (requiredSkill.CurrentLevel > 0)
            {
                return true;
            }
        }

        return false;
    }

    public string GetDescription()
    {
        string description;

        description = $"{data.Description} +{GetMultiplier() * 100}%";

        return description;
    }
}
